using System;
using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandCardData
    {
        
        public int Index;
        public CardStatsData Data;
        public HandCardView View;
        public ITransformCastCardBehaviour Behaviour;
        public GameplayLogicCard Logic;
        public bool IsLogicInitialized;
        
        public HandCardData(int index, CardStatsData data, HandCardView view, ITransformCastCardBehaviour behaviour, GameplayLogicCard logic)
        {
            Index = index;
            Data = data;
            View = view;
            Behaviour = behaviour;
        }
    }
}