using System;

namespace Feature.Card.Script
{
    public interface ITransformCastCardBehaviour
    {
        void TryCastCard(ITransformCastCardBehaviour _currentCardBehaviour);
        
        public bool _canCastCard { get;set; }
        
        event Action OnTryCardCast;
        void CanCastCard(bool b);
    }
}