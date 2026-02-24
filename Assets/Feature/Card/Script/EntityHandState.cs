using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class EntityHandState
    {
        public readonly List<HandCardData> HandData = new();
        public List<CardStatsData> PreviousCards = new();

        public EntityHandState(CardAndHealthEntityOwnerData owner)
        {
            Owner = owner;
        }

        public CardAndHealthEntityOwnerData Owner { get; }
    }
}