using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AIActionExecutor _actionExecutor;

        public AISystem(
            GameSessionModel gameSessionModel, 
            CardCastService cardCastService,
            ReadyStageBackOrFightSystem readyStageBackOrFightSystem, 
            HeroPowerSystem heroPowerSystem, 
            TargetingSystem targetingSystem, 
            AIActionExecutor actionExecutor)
        {
            _gameSessionModel = gameSessionModel;
            _cardCastService = cardCastService;
            _readyStageBackOrFightSystem = readyStageBackOrFightSystem;
            _heroPowerSystem = heroPowerSystem;
            _targetingSystem = targetingSystem;
            _actionExecutor = actionExecutor;
        }

        public void ExecutePreparePhase() => ExecuteNextAction();
        public void ExecuteFightPhase() => ExecuteNextAction();

        private void ExecuteNextAction()
        {
            var availableActions = CollectAvailableActions();

            if (availableActions.Count == 0)
            {
                EndTurn();
                return;
            }

            var chosenAction = PickRandomAction(availableActions);
            var target = PickRandomValidTarget(chosenAction);

            if (target == null)
            {
                ExecuteNextAction();
                return;
            }

            _actionExecutor.ExecuteDelayed(() =>
            {
                chosenAction.Execute(target);
                ExecuteNextAction();
            });
        }


        private List<IAIAction> CollectAvailableActions()
        {
            var actions = new List<IAIAction>();
            var aiPlayer = _gameSessionModel.EnemyHero;

            CollectCardActions(aiPlayer, actions);
            TryAddHeroPowerAction(aiPlayer, actions);

            return actions;
        }

        private void CollectCardActions(GameSessionPlayerData aiPlayer, List<IAIAction> actions)
        {
            bool hasFreeSlots = HasFreeBoardSlots(aiPlayer);

            foreach (var entityOwner in aiPlayer.CardAndHealthEntityOwners.ToList())
            {
                var affordableCards = GetAffordableCards(entityOwner);

                foreach (var card in affordableCards)
                {
                    if (ShouldSkipCard(card, hasFreeSlots))
                        continue;

                    var action = new CardAIAction(card, entityOwner, _cardCastService);

                    if (HasValidTargets(action))
                        actions.Add(action);
                }
            }
        }

        private void TryAddHeroPowerAction(GameSessionPlayerData aiPlayer, List<IAIAction> actions)
        {
            if (!CanUseHeroPower(aiPlayer))
                return;

            var action = new HeroPowerAIAction(
                aiPlayer.CurrentHeroPower, 
                aiPlayer.MainHeroEntity(),
                _cardCastService, 
                _gameSessionModel, 
                _heroPowerSystem);

            if (HasValidTargets(action))
                actions.Add(action);
        }

        private CardAndHealthEntityOwnerData PickRandomValidTarget(IAIAction action)
        {
            var validTargets = FilterValidTargets(action);
            
            return validTargets.Count > 0 
                ? validTargets[UnityEngine.Random.Range(0, validTargets.Count)] 
                : null;
        }

        private bool HasValidTargets(IAIAction action) =>
            FilterValidTargets(action).Count > 0;

        private List<CardAndHealthEntityOwnerData> FilterValidTargets(IAIAction action)
        {
            var allTargets = GetAllPossibleTargets();
            
            if (action.DealsDamage())
                allTargets = FilterEnemiesOnly(allTargets);
            
            if (_targetingSystem.IsPreparePhase)
                allTargets = FilterAlliesOnly(allTargets);
            
            return allTargets;
        }

        private List<CardAndHealthEntityOwnerData> GetAllPossibleTargets()
        {
            var targets = new List<CardAndHealthEntityOwnerData>();
            targets.AddRange(_gameSessionModel.PlayerHero.CardAndHealthEntityOwners);
            targets.AddRange(_gameSessionModel.EnemyHero.CardAndHealthEntityOwners);
            return targets;
        }

        private List<CardAndHealthEntityOwnerData> FilterEnemiesOnly(List<CardAndHealthEntityOwnerData> targets)
        {
            var aiOwners = _gameSessionModel.EnemyHero.CardAndHealthEntityOwners;
            return targets.Where(t => !aiOwners.Contains(t)).ToList();
        }

        private List<CardAndHealthEntityOwnerData> FilterAlliesOnly(List<CardAndHealthEntityOwnerData> targets)
        {
            var aiOwners = _gameSessionModel.EnemyHero.CardAndHealthEntityOwners;
            return targets.Where(t => aiOwners.Contains(t)).ToList();
        }
        
        private bool ShouldSkipCard(CardStatsData card, bool hasFreeSlots) => 
            card is MinionCardData && !hasFreeSlots;

        private List<CardStatsData> GetAffordableCards(CardAndHealthEntityOwnerData owner)
        {
            return owner.CardsInHand.CurrentValue
                .Where(card => card.Cost <= owner.Chakra)
                .ToList();
        }

        private bool HasFreeBoardSlots(GameSessionPlayerData player)
        {
            int occupiedSlots = player.CardsInBoard.CurrentValue.Count(c => c != null);
            return occupiedSlots < player.CardsInBoardMax;
        }

        private bool CanUseHeroPower(GameSessionPlayerData player)
        {
            var heroPower = player.CurrentHeroPower;
            var owner = player.MainHeroEntity();

            return heroPower != null
                   && !player.HeroPowerUsage.IsUsed(0) // ← индекс 0 для первой силы врага
                   && heroPower.Cost <= owner.Chakra;
        }

        private IAIAction PickRandomAction(List<IAIAction> actions) => 
            actions[UnityEngine.Random.Range(0, actions.Count)];

        private void EndTurn() => _readyStageBackOrFightSystem.SetEnemyReady();
    }
}