using System;
using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class HandCardData
    {
        public int Index;
        public CardStatsData Data;
        public CardView View;
        public ITargetCardBehaviour Behaviour;

        public HandCardData(int index, CardStatsData data, CardView view, ITargetCardBehaviour behaviour)
        {
            Index = index;
            Data = data;
            View = view;
            Behaviour = behaviour;
        }
    }
}