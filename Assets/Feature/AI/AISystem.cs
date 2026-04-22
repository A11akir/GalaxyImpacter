
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.StagesGameLogic;

namespace Feature.AI
{
    public class AISystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly CardCastService _cardCastService;
        private readonly ReadyStageBackOrFightSystem _readyStageBackOrFightSystem;

        public AISystem(GameSessionModel gameSessionModel, CardCastService cardCastService, ReadyStageBackOrFightSystem readyStageBackOrFightSystem)
        {
            _gameSessionModel = gameSessionModel;
            _cardCastService = cardCastService;
            _readyStageBackOrFightSystem = readyStageBackOrFightSystem;
        }

        public void ExecutePreparePhase()
        {
            ExecuteNextPrepareAction();
        }

        private void ExecuteNextPrepareAction()
        {
            var actions = GetPrepareActions();

            if (actions.Count == 0)
            {
                _readyStageBackOrFightSystem.SetEnemyReady();
                return;
            }
            

            var action = PickRandom(actions);
            float delay = UnityEngine.Random.Range(0.5f, 1.5f);

            DOVirtual.DelayedCall(delay, () =>
            {
                action.Execute(GetRandomTarget());
                ExecuteNextPrepareAction();
            });
        }
        
        private List<IAIAction> GetPrepareActions()
        {
            return GetAvailableActions();
        }

        private IAIAction PickRandom(List<IAIAction> actions)
        {
            int index = UnityEngine.Random.Range(0, actions.Count);
            return actions[index];
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
                actions.Add(new HeroPowerAIAction(enemy.CurrentHeroPower, enemy.MainHeroEntity(), _cardCastService));

            return actions;
        }

        private CardAndHealthEntityOwnerData GetRandomTarget()
        {
            var targets = new List<CardAndHealthEntityOwnerData>();
    
            targets.AddRange(_gameSessionModel.PlayerHero.CardAndHealthEntityOwners);
    
            targets.AddRange(_gameSessionModel.EnemyHero.CardAndHealthEntityOwners);
    
            return targets[UnityEngine.Random.Range(0, targets.Count)];
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

        public void ExecuteFightPhase()
        {
        }
    }
}