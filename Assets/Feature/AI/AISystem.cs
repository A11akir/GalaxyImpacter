using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.AI
{
    public class AISystem
    {
        private GameSessionModel _gameSessionModel;
        private CombatSystem.CombatSystem _combatSystem;
        private BattlefieldSystem _battlefieldSystem;

        public AISystem(GameSessionModel gameSessionModel, CombatSystem.CombatSystem combatSystem, BattlefieldSystem battlefieldSystem)
        {
            _gameSessionModel = gameSessionModel;
            _combatSystem = combatSystem;
            _battlefieldSystem = battlefieldSystem;
        }

        public void ExecutePreparePhase()
        {
            var actions = GetAvailableActions()
                .Where(a => !(a is CardAIAction card && card.TargetType == TargetType.AnyTarget))
                .ToList();
            
            if (actions.Count > 0)
                actions[0].Execute(null, _gameSessionModel);
        }

        public void ExecuteFightPhase()
        {
            
        }
        
        private List<IAIAction> GetAvailableActions()
        {
            var actions = new List<IAIAction>();
            var enemy = _gameSessionModel.EnemyHero;

            foreach (var owner in enemy.CardAndHealthEntityOwners)
            {
        
                var playableCards = owner.CardsInHand.CurrentValue
                    .Where(c => c.Cost <= owner.Chakra)
                    .ToList();

                foreach (var card in playableCards)
                {
                    actions.Add(new CardAIAction(card, owner, _battlefieldSystem, _combatSystem));
                }
            }

            if (CanUseHeroPower())
            {
                actions.Add(new HeroPowerAIAction(enemy.CurrentHeroPower, enemy.MainHeroEntity()));
            }
            
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
    }
}