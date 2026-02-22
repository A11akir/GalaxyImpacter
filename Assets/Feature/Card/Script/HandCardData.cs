using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public class HandCardData
    {
        public CardStatsData Data;
        public HandCardView View;
        public ITransformCastCardBehaviour Behaviour;
        public GameplayLogicCard Logic;
        
        public HandCardData( CardStatsData data, HandCardView view, ITransformCastCardBehaviour behaviour, GameplayLogicCard logic)
        {
            Logic = logic;
            Data = data;
            View = view;
            Behaviour = behaviour;
        }
    }
}