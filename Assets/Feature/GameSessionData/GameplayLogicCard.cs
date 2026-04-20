using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.GameSessionData
{
    public class GameplayLogicCard
    {
        private HandCardData _cardData;
        private GameSessionModel _gameSessionModel;
        private BattlefieldSystem _battlefieldSystem;
        private CombatSystem.CombatSystem _combatSystem;

        public GameplayLogicCard(HandCardData cardData, GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem,
            CombatSystem.CombatSystem combatSystem)
        {
            _cardData = cardData;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
            _combatSystem = combatSystem;
        }

        public void CastCard(CardAndHealthEntityOwnerData owner, CardAndHealthEntityOwnerData target)
        {
            if (!CheckCanCast(owner)) return;
            owner.Chakra -= _cardData.Data.Cost;

            if (_cardData.Data is MinionCardData)
                SpawnHeroCard(owner);
            else if (_cardData.Data is SpellCardData spell)
                CastSpell(spell, owner, target);
            
            if (owner.CardsInHand.CurrentValue.Contains(_cardData.Data))
                owner.RemoveCardFromHand(_cardData.Data);
        }

        private void CastSpell(SpellCardData spell, CardAndHealthEntityOwnerData owner, CardAndHealthEntityOwnerData target)
        {
            for (int i = 0; i < spell.Effects.Count; i++)
            {
                var context = new EffectContext
                {
                    Caster = owner,
                    Target = target,
                    GameSessionModel = _gameSessionModel,
                    CombatSystem = _combatSystem,
                    BattlefieldSystem = _battlefieldSystem,
                    CardData = spell,
                    ValueIndex = i
                };
        
                spell.Effects[i].Execute(context);
            }
        }

        private bool CheckCanCast(CardAndHealthEntityOwnerData owner)
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(owner);
    
            if (_cardData.Data.IsHero && 
                playerData.CardsInBoard.CurrentValue.Count > playerData.CardsInBoardMax)
                return false;

            if (owner.Chakra < _cardData.Data.Cost)
                return false;

            return true;
        }
        
        

        private void SpawnHeroCard(CardAndHealthEntityOwnerData owner)
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(owner);
            _battlefieldSystem.AddCardInBattlefield(playerData, _cardData.Data);
        }
    }
}