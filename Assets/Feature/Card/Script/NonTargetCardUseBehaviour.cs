using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Feature.Card.Script
{
    public class NonTargetCardUseBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler, ITargetCardBehaviour
    {
        [Inject] CastCardAreaAllTarget _castCardAreaAllTarget;
        
        public void OnDrag(PointerEventData eventData)
        {
            _castCardAreaAllTarget.CheckCardArea();
            _castCardAreaAllTarget.CardGoingIsUsed = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _castCardAreaAllTarget.CardIsAreaAllTargetUseEffectOff();
            _castCardAreaAllTarget.CardGoingIsUsed = false;
            
            TryCastCard(this);
        }

        public void TryCastCard(ITargetCardBehaviour _currentCardBehaviour)
        {
            if (_castCardAreaAllTarget.CardHasTarget)
            {
                OnTryCardCast?.Invoke();
            }
        }

        public event Action OnTryCardCast;
    }
}