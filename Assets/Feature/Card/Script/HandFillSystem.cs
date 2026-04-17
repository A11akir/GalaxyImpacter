using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandFillSystem
    {
        private readonly GameSessionModel _gameSessionModel;

        public HandFillSystem(GameSessionModel gameSessionModel) => _gameSessionModel = gameSessionModel;

        public void FillHandDataInDecks()
        {
            foreach (var entity in _gameSessionModel.GetAllEntityOwners())
            {
                FillHandFromDeck(entity);
            }
        }

        public void FillEntityHand(CardAndHealthEntityOwnerData entity)
        {
            FillHandFromDeck(entity);
        }

        private void FillHandFromDeck(CardAndHealthEntityOwnerData entity)
        {
            entity.ShuffleDeck();

            for (int i = 0; i < entity.startCardsInHandToDraw; i++)
            {
                var drawnCard = entity.DrawCardFromDeck();
                if (drawnCard == null)
                    break;

                entity.AddCardToHand(drawnCard, entity.CountCardsInHand);
            }

            Debug.Log($"Entity: взято {entity.CardsInHand.CurrentValue.Count} карт, " +
                      $"в колоде осталось {entity.CardsInDeck.CurrentValue.Count}");
        }
    }
}