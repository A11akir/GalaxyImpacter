using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.Hero;
using Feature.StagesGameLogic;

namespace Feature.AI
{
    public class AISystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly CardCastService _cardCastService;
        private readonly ReadyStageBackOrFightSystem _readyStageBackOrFightSystem;
        private readonly HeroPowerSystem _heroPowerSystem;
        private readonly TargetingSystem _targetingSystem;

        public AISystem(GameSessionModel gameSessionModel, CardCastService cardCastService,
            ReadyStageBackOrFightSystem readyStageBackOrFightSystem, HeroPowerSystem heroPowerSystem, TargetingSystem targetingSystem)
        {
            _gameSessionModel = gameSessionModel;
            _cardCastService = cardCastService;
            _readyStageBackOrFightSystem = readyStageBackOrFightSystem;
            _heroPowerSystem = heroPowerSystem;
            _targetingSystem = targetingSystem;
        }

        public void ExecutePreparePhase() => ExecuteNextAction(GetAvailableActions);
        public void ExecuteFightPhase() => ExecuteNextAction(GetAvailableActions);

        private void ExecuteNextAction(Func<List<IAIAction>> getActions)
        {
            var actions = getActions();

            if (actions.Count == 0)
            {
                _readyStageBackOrFightSystem.SetEnemyReady();
                return;
            }

            var action = PickRandom(actions);
            var target = GetRandomTarget(action);
    
            if (target == null)
            {
                ExecuteNextAction(getActions);
                return;
            }

            float delay = UnityEngine.Random.Range(0.5f, 1.5f);

            DOVirtual.DelayedCall(delay, () =>
            {
                action.Execute(target);
                ExecuteNextAction(getActions);
            });
        }

        private List<IAIAction> GetAvailableActions()
        {
            var actions = new List<IAIAction>();
            var enemy = _gameSessionModel.EnemyHero;

            foreach (var owner in enemy.CardAndHealthEntityOwners.ToList())
            {
                var playableCards = owner.CardsInHand.CurrentValue
                    .Where(c => c.Cost <= owner.Chakra)
                    .ToList();

                foreach (var card in playableCards)
                    actions.Add(new CardAIAction(card, owner, _cardCastService));
            }

            if (CanUseHeroPower())
                actions.Add(new HeroPowerAIAction(enemy.CurrentHeroPower, enemy.MainHeroEntity(),
                    _cardCastService, _gameSessionModel, _heroPowerSystem));

            return actions;
        }

        private IAIAction PickRandom(List<IAIAction> actions) =>
            actions[UnityEngine.Random.Range(0, actions.Count)];

        private CardAndHealthEntityOwnerData GetRandomTarget(IAIAction action)
        {
            var targets = GetAllPossibleTargets();
            
            if (action.DealsDamage())
            {
                var enemy = _gameSessionModel.EnemyHero;
                targets = targets
                    .Where(t => !enemy.CardAndHealthEntityOwners.Contains(t))
                    .ToList();
            }
    
            if (targets.Count == 0)
                return null;
    
            return targets[UnityEngine.Random.Range(0, targets.Count)];
        }

        private List<CardAndHealthEntityOwnerData> GetAllPossibleTargets()
        {
            var all = new List<CardAndHealthEntityOwnerData>();
            all.AddRange(_gameSessionModel.PlayerHero.CardAndHealthEntityOwners);
            all.AddRange(_gameSessionModel.EnemyHero.CardAndHealthEntityOwners);
            return all;
        }

        private bool CanUseHeroPower()
        {
            var enemy = _gameSessionModel.EnemyHero;
            var owner = enemy.MainHeroEntity();
            var heroPower = enemy.CurrentHeroPower;

            if (heroPower == null) return false;
            if (enemy.HeroPowerUsedThisTurn) return false;
            if (heroPower.Cost > owner.Chakra) return false;

            return true;
        }
    }
}