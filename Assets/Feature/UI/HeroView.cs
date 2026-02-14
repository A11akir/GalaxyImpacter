using System;
using Feature.GameSessionData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.UI
{
    public class HeroView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _wasSelectBotWindow;
        [SerializeField] private GameObject _selectWindow;      
        [SerializeField] private GameObject _nameWindow;
        [SerializeField] private GameObject _banWindow;
        
        [SerializeField] private Image _iconImage;
        
        [SerializeField] public TextMeshProUGUI _heroPowerText;
        [SerializeField] public TextMeshProUGUI _healthText;
        [SerializeField] public TextMeshProUGUI _nameText;

        public bool _isBlockedForSelect;
        public GameSessionPlayerData HeroData { get; private set; }
        public event Action<HeroView> OnSelectHeroView;
        
        public void SetViewData(GameSessionPlayerData data)
        {
            HeroData = data;
            _iconImage.sprite = data._iconImage;
            _nameText.text = data._heroName;
            _healthText.text = data._health.ToString();
            _heroPowerText.text = data._heroPowerCost.ToString();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isBlockedForSelect) return;
            OnSelectHeroView?.Invoke(this);
        }

        public void SelectHeroView()
        {
            _selectWindow.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData) => _nameWindow.SetActive(true);

        public void OnPointerExit(PointerEventData eventData) => _nameWindow.SetActive(false);

        public void BanHeroView()
        {
            _isBlockedForSelect = true;
            _banWindow.SetActive(true);
            ClearSelectWindow();
        }

        public void ClearSelectWindow() => _selectWindow.SetActive(false);

        public void WasSetHeroEnemy()
        {
            _wasSelectBotWindow.SetActive(true);
            _isBlockedForSelect = true;
        }
    }
}