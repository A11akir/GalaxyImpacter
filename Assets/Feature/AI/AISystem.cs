using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.Hero;
using Feature.StagesGameLogic;
using UnityEngine;

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
    
            int occupiedSlots = enemy.CardsInBoard.CurrentValue.Count(c => c != null);
            bool hasFreeSlots = occupiedSlots < enemy.CardsInBoardMax;

            foreach (var owner in enemy.CardAndHealthEntityOwners.ToList())
            {
                var playableCards = owner.CardsInHand.CurrentValue
                    .Where(c => c.Cost <= owner.Chakra)
                    .ToList();

                foreach (var card in playableCards)
                {
                    if (card is MinionCardData && !hasFreeSlots)
                        continue;
            
                    var action = new CardAIAction(card, owner, _cardCastService);
            
                    if (HasValidTarget(action))
                        actions.Add(action);
                    else
                        Debug.Log($"[AI] Skipping '{card.Name}' - no valid targets");
                }
            }

            if (CanUseHeroPower())
            {
                var heroPowerAction = new HeroPowerAIAction(enemy.CurrentHeroPower, enemy.MainHeroEntity(),
                    _cardCastService, _gameSessionModel, _heroPowerSystem);
        
                if (HasValidTarget(heroPowerAction))
                    actions.Add(heroPowerAction);
                else
                    Debug.Log($"[AI] Skipping HeroPower - no valid targets");
            }

            return actions;
        }

        private bool HasValidTarget(IAIAction action)
        {
            var availableTargets = GetAllPossibleTargets();
    
            if (action.DealsDamage())
            {
                var enemy = _gameSessionModel.EnemyHero;
                availableTargets = availableTargets
                    .Where(t => !enemy.CardAndHealthEntityOwners.Contains(t))
                    .ToList();
            }
    
            if (_targetingSystem.IsPreparePhase)
            {
                var enemy = _gameSessionModel.EnemyHero;
                availableTargets = availableTargets
                    .Where(t => enemy.CardAndHealthEntityOwners.Contains(t))
                    .ToList();
            }
    
            return availableTargets.Count > 0;
        }

        private IAIAction PickRandom(List<IAIAction> actions) =>
            actions[UnityEngine.Random.Range(0, actions.Count)];

        private CardAndHealthEntityOwnerData GetRandomTarget(IAIAction action)
        {
            var availableTargets = GetAllPossibleTargets();
    
            if (action.DealsDamage())
            {
                var enemy = _gameSessionModel.EnemyHero;
                availableTargets = availableTargets
                    .Where(t => !enemy.CardAndHealthEntityOwners.Contains(t))
                    .ToList();
            }
    
            if (_targetingSystem.IsPreparePhase)
            {
                var enemy = _gameSessionModel.EnemyHero;
                availableTargets = availableTargets
                    .Where(t => enemy.CardAndHealthEntityOwners.Contains(t))
                    .ToList();
            }
    
            if (availableTargets.Count == 0)
                return null;
    
            return availableTargets[UnityEngine.Random.Range(0, availableTargets.Count)];
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