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

        public void CastCard()
        {
            if (!CheckCanCast()) return;
            
            _gameSessionModel.PlayerHero.Chakra -= _cardData.Data.Cost;
            
            if (_cardData.Data.IsHero) SpawnHeroCard();
            else CastSpell();
        }

        private bool CheckCanCast()
        {
            if (_cardData.Data.IsHero && 
                _gameSessionModel.PlayerHero.CardsInBoard.CurrentValue.Count 
                > _gameSessionModel.PlayerHero.CardsInBoardMax)
                return false;

            if (_gameSessionModel.PlayerHero.Chakra < _cardData.Data.Cost)
                return false;

            return true;
        }

        private void CastSpell()
        {
            
        }

        private void SpawnHeroCard()
        {
            _battlefieldSystem.AddCardInBattlefield(_gameSessionModel.PlayerHero, _cardData.Data);
            _gameSessionModel.PlayerHero.RemoveCardFromHand(_cardData);
        }
    }
}