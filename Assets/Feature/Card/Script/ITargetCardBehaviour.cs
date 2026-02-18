using System;

namespace Feature.Card.Script
{
    public interface ITargetCardBehaviour
    {
        void TryCastCard(ITargetCardBehaviour _currentCardBehaviour);
        
        event Action OnTryCardCast;
    }
}