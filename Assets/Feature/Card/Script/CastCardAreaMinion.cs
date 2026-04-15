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
        [SerializeField] GameObject CardIsAreaBattlefieldHero;
        [SerializeField] GameObject CardIsAreaBattlefieldSpell;
        [Inject] private TipPlaceBattlefieldViewSystem _tipPlaceBattlefieldViewSystem;
        [HideInInspector] public bool CardGoingIsUsed;
        [HideInInspector] public bool CardHasTarget;
        [SerializeField] private GraphicRaycaster _raycaster;
        [SerializeField] private EventSystem _eventSystem;

        public void CheckCardAreaSpell(Transform transformCard)
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

                CardIsAreaBattlefieldSpell.SetActive(mouseOverThis);

                /*if (mouseOverThis)
                    _tipPlaceBattlefieldViewSystem.ActiveNearTip(transformCard);
                else
                    _tipPlaceBattlefieldViewSystem.Inactive();*/

                CardHasTarget = mouseOverThis;
            }
            else
            {
                CardHasTarget = false;
                CardIsAreaBattlefieldSpell.SetActive(false);
                _tipPlaceBattlefieldViewSystem.Inactive();
            }
        }

        public void CardIsAreaAllTargetUseEffectOff()
        {
            CardHasTarget = false;
            CardIsAreaBattlefieldSpell.SetActive(false);
            CardIsAreaBattlefieldHero.SetActive(false);
            _tipPlaceBattlefieldViewSystem.Inactive();

        }

        public void CheckCardAreaHero(Transform transformCard)
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

                CardIsAreaBattlefieldHero.SetActive(mouseOverThis);

                if (mouseOverThis)
                    _tipPlaceBattlefieldViewSystem.ActiveNearTip(transformCard);
                else
                    _tipPlaceBattlefieldViewSystem.Inactive();

                CardHasTarget = mouseOverThis;
            }
        }
    }
}