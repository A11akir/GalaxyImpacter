using System;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    public interface ITransformCastCardBehaviour
    {
        public bool _canCastCard { get; set; }
        event Action<CardAndHealthEntityOwnerData> OnTryCardCast;
        void TryCastCard(ITransformCastCardBehaviour _currentCardBehaviour);
        void CanCastCard(bool b);
        void SetOwner(CardAndHealthEntityOwnerData owner);
    }
}