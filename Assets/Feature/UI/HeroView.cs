using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.UI
{
    public class HeroView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _banWindow;
        [SerializeField] private GameObject _nameWindow;
        
        [SerializeField] private GameObject _selectWindow;
        
        [SerializeField] public TextMeshProUGUI _nameText;
        [SerializeField] public TextMeshProUGUI _healthText;
        [SerializeField] public TextMeshProUGUI _heroPowerText;
        
        private bool _isBanned;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isBanned) return;
            _selectWindow.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _nameWindow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _nameWindow.SetActive(false);
        }

        public void BanHero()
        {
            _banWindow.SetActive(true);
        }

        public void ClearSelectWindow()
        {
            _selectWindow.SetActive(false);
        }

        public void ExitSelectMode()
        {
            _selectWindow.SetActive(false);
            _banWindow.SetActive(false);
        }
    }
}