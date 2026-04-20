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
        public bool IsHiddenForEnemyPlayer;

        public EntityHandState(CardAndHealthEntityOwnerData owner, HandCardViews handCardViews, bool isHiddenForEnemyPlayer = false)
        {
            Owner = owner;
            HandCardViews = handCardViews;
            IsHiddenForEnemyPlayer = isHiddenForEnemyPlayer;
        }
    }
}