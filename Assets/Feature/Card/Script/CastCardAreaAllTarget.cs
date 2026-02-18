using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Card.Script
{
    public class CastCardAreaAllTarget : MonoBehaviour
    {
        [SerializeField] GameObject CardIsAreaAllTargetUseEffect;
        [HideInInspector] public bool CardGoingIsUsed;

        public bool CardHasTarget;
        private GraphicRaycaster _raycaster;
        private EventSystem _eventSystem;
        
        private void Start()
        {
            _eventSystem = EventSystem.current;
            _raycaster = FindObjectOfType<GraphicRaycaster>();
        }

        public void CheckCardArea()
        {
            if (CardGoingIsUsed && _raycaster && _eventSystem)
            {
                PointerEventData pointerData = new PointerEventData(_eventSystem);
                pointerData.position = Input.mousePosition;
                
                var results = new System.Collections.Generic.List<RaycastResult>();
                _raycaster.Raycast(pointerData, results);
                
                bool mouseOverThis = false;
                foreach (var result in results)
                {
                    if (result.gameObject == gameObject)
                    {
                        mouseOverThis = true;
                        break;
                    }
                }
                CardIsAreaAllTargetUseEffect.SetActive(mouseOverThis);
                CardHasTarget =  true;
            }
            else
            {
                CardIsAreaAllTargetUseEffect.SetActive(false);
                CardHasTarget =  false;
            }
        }
        
        public void CardIsAreaAllTargetUseEffectOff()
        {
            CardIsAreaAllTargetUseEffect.SetActive(false);
            CardHasTarget =  false;
        }
    }
}