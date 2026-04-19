using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Hero
{
    public class HeroPowerView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _heroPowerDescriptionWindow;
        
        public void OnPointerEnter(PointerEventData eventData) => _heroPowerDescriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => _heroPowerDescriptionWindow.SetActive(false);
    }
}