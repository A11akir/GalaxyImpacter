using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Feature.Card.Script
{
    public class NonTargetCardUseBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [Inject] CastCardAreaAllTarget _castCardAreaAllTarget;
        
        public void OnDrag(PointerEventData eventData)
        {
            _castCardAreaAllTarget.CardGoingIsUsed = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _castCardAreaAllTarget.CardIsAreaAllTargetUseEffectOff();
            _castCardAreaAllTarget.CardGoingIsUsed = false;
        }
    }
}