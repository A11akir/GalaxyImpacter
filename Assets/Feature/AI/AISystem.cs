using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.AI
{
    public class AISystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly CardCastService _cardCastService;

        public AISystem(GameSessionModel gameSessionModel, CardCastService cardCastService)
        {
            _gameSessionModel = gameSessionModel;
            _cardCastService = cardCastService;
        }

        public void ExecutePreparePhase()
        {
            ExecuteNextPrepareAction();
        }

        private void ExecuteNextPrepareAction()
        {
            var actions = GetPrepareActions();
            
            if (actions.Count == 0) return;

            var action = PickRandom(actions);
            float delay = UnityEngine.Random.Range(0.5f, 1.5f);

            DOVirtual.DelayedCall(delay, () =>
            {
                action.Execute(null);
                ExecuteNextPrepareAction();
            });
        }
        
        private List<IAIAction> GetPrepareActions()
        {
            return GetAvailableActions()
                .Where(a => a.TargetType != TargetType.AnyTarget)
                .ToList();
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