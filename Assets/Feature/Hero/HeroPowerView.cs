using Feature.Card.Script;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Hero
{
    public class HeroPowerView : HandCardView, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _heroPowerDescriptionWindow;
        [SerializeField] private GameObject chakraCostFrame;
        [SerializeField] private GameObject _heroPowerUsedThisTurnWindow;
        [SerializeField] private GameObject _canAvailableCastHeroPower;
        public void OnPointerEnter(PointerEventData eventData) => _heroPowerDescriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => _heroPowerDescriptionWindow.SetActive(false);


        public void SetCanCastView(bool canCast)
        {
            _canAvailableCastHeroPower.SetActive(canCast);
        }

        public void SetUsedThisTurnView(bool usedThisTurn)
        {
            _heroPowerUsedThisTurnWindow.SetActive(usedThisTurn);
            chakraCostFrame.SetActive(!usedThisTurn);
        }
    }
}