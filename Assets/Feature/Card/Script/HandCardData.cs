using Feature.GameSessionData;
using R3;

namespace Feature.Card.Script
{
    public class HandCardData
    {
        public CardStatsData Data;
        public HandCardView View;
        public ITransformCastCardBehaviour Behaviour;
        public HandCardCastHandler Logic;
        public readonly CompositeDisposable Subscriptions = new();

        public HandCardData(CardStatsData data, HandCardView view, ITransformCastCardBehaviour behaviour, HandCardCastHandler logic)
        {
            Logic = logic;
            Data = data;
            View = view;
            Behaviour = behaviour;
        }
    }
}