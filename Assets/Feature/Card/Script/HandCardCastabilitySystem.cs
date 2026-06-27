// HandCardCastabilitySystem.cs — без лишних зависимостей
using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class HandCardCastabilitySystem
    {
        public void Refresh(HandCardData cardData, int chakra)
        {
            bool canCast = chakra >= cardData.Data.Cost;
            cardData.Behaviour.CanCastCard(canCast);
            cardData.View.SetCanCastView(canCast);
        }

        public void RefreshHand(List<HandCardData> handData, int chakra)
        {
            foreach (var cardData in handData)
                Refresh(cardData, chakra);
        }
    }
}