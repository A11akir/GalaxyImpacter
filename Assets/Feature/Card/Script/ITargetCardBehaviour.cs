using System;

namespace Feature.Card.Script
{
    public interface ITargetCardBehaviour
    {
        void TryCastCard(ITargetCardBehaviour _currentCardBehaviour);
        
        public bool _canCastCard { get;set; }
        
        event Action OnTryCardCast;
        void CanCastCard(bool b);
    }
}