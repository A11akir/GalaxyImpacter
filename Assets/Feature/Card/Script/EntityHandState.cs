using System.Collections.Generic;
using Feature.GameSessionData;
using R3;

namespace Feature.Card.Script
{
    public class EntityHandState
    {
        public CardAndHealthEntityOwnerData Owner;
        public HandCardViews HandCardViews;
        public List<HandCardData> HandData = new();
        public List<CardStatsData> PreviousCards = new();
        public CompositeDisposable Disposables = new();

        public EntityHandState(CardAndHealthEntityOwnerData owner, HandCardViews handCardViews)
        {
            Owner = owner;
            HandCardViews = handCardViews;
        }
    }
}