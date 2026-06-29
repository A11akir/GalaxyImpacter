using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.Health;
using Feature.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ITargetable = Feature.GameSessionData.ITargetable;

namespace Feature.UI
{
    public class HeroView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, ITargetable, IEntityView
    {
        [SerializeField] private List<HeroPowerPreview> heroPowerPreviewViews;
        [SerializeField] public List<HeroPowerGameplayView> heroPowerGameplayViews; 
        
        [SerializeField] private GameObject wasSelectBotWindow;
        [SerializeField] private GameObject _selectWindow;      
        [SerializeField] private GameObject _nameWindow;
        [SerializeField] private GameObject _banWindow;
        
        [SerializeField] private Image _iconImage;
        
        [SerializeField] private GameObject _armorRoot;
        [SerializeField] private TextMeshProUGUI _armorText;
        [SerializeField] public TextMeshProUGUI _healthText;
        [SerializeField] public TextMeshProUGUI _nameText;
        
        [SerializeField] private PassiveEffectsContainerView _passiveEffectsView;
        public PassiveEffectsContainerView PassiveEffectsView => _passiveEffectsView;
        
        public bool _isBlockedForSelect;
        public GameSessionPlayerData HeroData { get; private set; }
        
        public event Action<HeroView> OnSelectHeroView;
        public event Action OnEntityClicked;
        
        private bool _isGameplayMode;

        public void SetViewData(GameSessionPlayerData data)
        {
            HeroData = data;
            _iconImage.sprite = data._iconImage;
            _nameText.text = data.MainHeroEntity()._heroName;
            _healthText.text = data.MainHeroEntity().HealthValue.ToString();

            SetHeroPowerViews(heroPowerPreviewViews, data);
        }

        private void SetHeroPowerViews<T>(List<T> views, GameSessionPlayerData data)
            where T : HeroPowerPreview
        {
            if (data.HeroPowers == null) return;

            for (int i = 0; i < views.Count; i++)
            {
                bool hasPower = i < data.HeroPowers.Count;
                views[i].gameObject.SetActive(hasPower);

                if (hasPower)
                    views[i].SetDataView(data.HeroPowers[i]);
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
        
        public void SetArmor(int armor)
        {
            bool hasArmor = armor > 0;

            _armorRoot.SetActive(hasArmor);

            _armorText.text = hasArmor 
                ? armor.ToString() 
                : "";
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _nameWindow.SetActive(true);
            _passiveEffectsView.SetHovered(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _nameWindow.SetActive(false);
            _passiveEffectsView.SetHovered(false);
        }

        public void SetGameplayMode(bool isGameplay)
        {
            _isGameplayMode = isGameplay;
            foreach (var heroPowerPreview in heroPowerPreviewViews)
                heroPowerPreview.gameObject.SetActive(false);

            if (isGameplay && HeroData != null)
                SetHeroPowerViews(heroPowerGameplayViews, HeroData);

        }

        public void BanHeroView()
        {
            _isBlockedForSelect = true;
            _banWindow.SetActive(true);
            ClearSelectWindow();
        }

        public void ClearSelectWindow() => _selectWindow.SetActive(false);

        public void WasSetHeroEnemy()
        {
            wasSelectBotWindow.SetActive(true);
            _isBlockedForSelect = true;
        }
        
        public void SetHealth(int hp) => _healthText.text = hp.ToString();


    }
}