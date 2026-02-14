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
        [SerializeField] private GameObject _banWindow;
        [SerializeField] private GameObject _nameWindow;
        
        [SerializeField] private Image _iconImage;
        
        [SerializeField] private GameObject _selectWindow;      
        [SerializeField] private GameObject _wasSelectBotWindow;
        
        [SerializeField] public TextMeshProUGUI _nameText;
        [SerializeField] public TextMeshProUGUI _healthText;
        [SerializeField] public TextMeshProUGUI _heroPowerText;

        public bool _isBlocked;

        public GameSessionPlayerData HeroData { get; private set; }
        
        public event Action<HeroView> OnSelectHeroView;
        
        public void SetData(GameSessionPlayerData data)
        {
            HeroData = data;
            _iconImage.sprite = data._iconImage;
            _nameText.text = data._heroName;
            _healthText.text = data._health.ToString();
            _heroPowerText.text = data._heroPowerCost.ToString();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isBlocked) return;
            OnSelectHeroView?.Invoke(this);
            _selectWindow.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData) => _nameWindow.SetActive(true);

        public void OnPointerExit(PointerEventData eventData) => _nameWindow.SetActive(false);

        public void BanHero()
        {
            _isBlocked = true;
            _banWindow.SetActive(true);
            ClearSelectWindow();
        }

        public void ClearSelectWindow() => _selectWindow.SetActive(false);

        public void ExitSelectMode()
        {
            _selectWindow.SetActive(false);
            _banWindow.SetActive(false);
            _wasSelectBotWindow.SetActive(false);
        }

        public void WasSetHeroEnemy()
        {
            _wasSelectBotWindow.SetActive(true);
            _isBlocked = true;
        }
    }
}