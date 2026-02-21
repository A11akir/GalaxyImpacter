using Feature.GameSessionData;
using System.Collections.Generic;
using Feature.Hero;
using UnityEngine;
using R3;

namespace Feature.Card.Script
{
    public class HandFillSystem
    {
        private GameSessionModel _gameSessionModel;

        public HandFillSystem(GameSessionModel gameSessionModel) => _gameSessionModel = gameSessionModel;

        public void FillHandDataInDecks()
        {
            FillHandDataForHeroFromDeck(_gameSessionModel.PlayerHero);
            FillHandDataForHeroFromDeck(_gameSessionModel.EnemyHero);
        }

        private void FillHandDataForHeroFromDeck(GameSessionPlayerData hero)
        {
            hero.ShuffleDeck();

            for (int i = 0; i < hero.startCardsInHandToDraw; i++)
            {
                hero.AddCardToHand(hero.DrawCardFromDeck(), hero.CountCardsInHand);
            }

            Debug.Log($"Рука {hero._heroName}: взято {hero.CardsInHand.CurrentValue.Count}" +
                      $" карт, в колоде осталось {hero.CardsInDeck.CurrentValue.Count}");
        }
    }
}