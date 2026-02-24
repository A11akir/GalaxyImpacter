using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using UnityEngine;

namespace Feature.GameSessionData
{
    public class GameplayLogicCard
    {
        private HandCardData _cardData;
        private GameSessionModel _gameSessionModel;
        private BattlefieldSystem _battlefieldSystem;

        public GameplayLogicCard(HandCardData cardData, GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem)
        {
            _cardData = cardData;
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
        }

        public void CastCard(CardAndHealthEntityOwnerData owner)
        {
            if (!CheckCanCast(owner)) return;
    
            owner.Chakra -= _cardData.Data.Cost;
    
            if (_cardData.Data.IsHero) SpawnHeroCard(owner);
            else CastSpell();
        }

        private void CastSpell()
        {
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