using Feature.Battlefield.Script;
using Feature.Battlefield.Script.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.Card.Script
{
    public class CastCardAreaMinion : MonoBehaviour
    {
        [SerializeField] GameObject CardIsAreaBattlefield;
        [Inject] private TipPlaceBattlefieldViewSystem _tipPlaceBattlefieldViewSystem;
        
        [HideInInspector] public bool CardGoingIsUsed;

        public bool CardHasTarget;
        private GraphicRaycaster _raycaster;
        private EventSystem _eventSystem;
        
        private void Start()
        {
            _eventSystem = EventSystem.current;
            _raycaster = FindObjectOfType<GraphicRaycaster>();
        }

        public void CheckCardArea(Transform transformCard)
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
                
                CardIsAreaBattlefield.SetActive(mouseOverThis);
                _tipPlaceBattlefieldViewSystem.ActiveNearTip(transformCard);
                CardHasTarget =  true;
            }
            else
            {
                CardIsAreaBattlefield.SetActive(false);
                _tipPlaceBattlefieldViewSystem.Inactive();
                CardHasTarget =  false;
            }
        }
        
        public void CardIsAreaAllTargetUseEffectOff()
        {
            CardIsAreaBattlefield.SetActive(false);
            _tipPlaceBattlefieldViewSystem.Inactive();
            CardHasTarget =  false;
        }
    }
}