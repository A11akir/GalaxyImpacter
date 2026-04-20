using Feature.Card.Script;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Hero
{
    public class HeroPowerView : HandCardView, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _heroPowerDescriptionWindow;
        
        [SerializeField] public GameObject _heroPowerArrowLine;
        [SerializeField] public GameObject _heroPowerArrowHead;
        [SerializeField] public GameObject _heroPowerContainer;
        
        public void OnPointerEnter(PointerEventData eventData) => _heroPowerDescriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => _heroPowerDescriptionWindow.SetActive(false);
    }
}