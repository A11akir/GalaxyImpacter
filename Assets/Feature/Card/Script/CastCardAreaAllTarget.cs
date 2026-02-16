using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Card.Script
{
    public class CastCardAreaAllTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject CardIsAreaAllTargetUseEffect;
        [HideInInspector] public bool CardGoingIsUsed;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (CardGoingIsUsed)
                CardIsAreaAllTargetUseEffect.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CardIsAreaAllTargetUseEffectOff();
        }

        public void CardIsAreaAllTargetUseEffectOff()
        {
            CardIsAreaAllTargetUseEffect.SetActive(false);
        }
    }
}