using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class EntityHandState
    {
        public CardAndHealthEntityOwnerData Owner;
        public HandCardViews HandCardViews;
        public List<HandCardData> HandData = new();
        public List<CardStatsData> PreviousCards = new();

        public EntityHandState(CardAndHealthEntityOwnerData owner, HandCardViews handCardViews)
        {
            Owner = owner;
            HandCardViews = handCardViews;
        }
    }
}