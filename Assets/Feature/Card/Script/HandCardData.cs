using System;
using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class HandCardData
    {
        public CardStatsData Data;
        public HandCardView View;
        public ITransformCastCardBehaviour Behaviour;
        public GameplayLogicCard Logic;
        public int Index;
        
        public HandCardData( CardStatsData data, HandCardView view, ITransformCastCardBehaviour behaviour, GameplayLogicCard logic, int index)
        {
            Logic = logic;
            Data = data;
            View = view;
            Behaviour = behaviour;
            Index = index;
        }
    }
}