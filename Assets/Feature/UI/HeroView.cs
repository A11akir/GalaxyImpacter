using System;
using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.Health;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.UI
{
    public class HeroView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IHealthView, ITargetable
    {
        [SerializeField] private List<HandCardView> _heroPowerCardView; 
        [SerializeField] private GameObject _wasSelectBotWindow;
        [SerializeField] private GameObject _selectWindow;      
        [SerializeField] private GameObject _nameWindow;
        [SerializeField] private GameObject _banWindow;
        
        [SerializeField] private Image _iconImage;
        [SerializeField] public Image _heroPowerIcon;
        [SerializeField] public TextMeshProUGUI _heroPowerText;        
        [SerializeField] public TextMeshProUGUI _healthText;
        [SerializeField] public TextMeshProUGUI _nameText;

        public bool _isBlockedForSelect;
        public GameSessionPlayerData HeroData { get; private set; }
        
        public event Action<HeroView> OnSelectHeroView;
        public event Action OnEntityClicked;
        
        private bool _isGameplayMode;

        public void SetGameplayMode(bool isGameplay) => _isGameplayMode = isGameplay;

        public void SetViewData(GameSessionPlayerData data)
        {
            HeroData = data;
            _iconImage.sprite = data._iconImage;
            _nameText.text = data.MainHeroEntity()._heroName;
            _heroPowerText.text = data.CurrentHeroPower?.Cost.ToString();
            _heroPowerIcon.sprite = data._heroPowerSprite;

            for (int i = 0; i < _heroPowerCardView.Count; i++)
            {
                if (data.HeroPowers != null && i < data.HeroPowers.Count)
                {
                    _heroPowerCardView[i].SetDataView(data.HeroPowers[i]);
                    _heroPowerCardView[i].gameObject.SetActive(false);
                }
                else
                {
                    _heroPowerCardView[i].gameObject.SetActive(false);
                }
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isGameplayMode)
            {
                OnEntityClicked?.Invoke();
                return;
            }

            if (!_isBlockedForSelect)
                OnSelectHeroView?.Invoke(this);
        }

        public void SetSelected(bool selected) => _selectWindow.SetActive(selected);
        public void SelectHeroView() => _selectWindow.SetActive(true);
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
        
        public void SetHealth(int hp) => _healthText.text = hp.ToString();
    }
}