using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroView : MonoBehaviour
    {
        [SerializeField] public List<HeroView> heroViews;
        [SerializeField] public Button buttonSelectHero;
        [SerializeField] public Button buttonBanHero;
        [SerializeField] public TextMeshProUGUI textSelectHero;

        [SerializeField] public List<GameObject> BlurObjects;
        
        [HideInInspector]public HeroView _selectHeroView;
        public event Action OnSelectWindowHeroView;
        public event Action OnChoseHeroButtonClicked;       
        public event Action OnBanHeroButtonClicked;

        public void Init()
        {
            buttonSelectHero.onClick.AddListener(() => OnChoseHeroButtonClicked?.Invoke());
            buttonBanHero.onClick.AddListener(() => OnBanHeroButtonClicked?.Invoke());

            SubscribeAllHeroViewForSelect();
        }

        private void SubscribeAllHeroViewForSelect()
        {
            foreach (var heroView in heroViews)
            {
                heroView.OnSelectHeroView += HeroViewViewOnOnSelectHeroView;
            }
        }

        private void HeroViewViewOnOnSelectHeroView(HeroView heroView)
        {
            _selectHeroView = heroView;
            ClearAllViewSelected();
            _selectHeroView.SelectHeroView();
            
            OnSelectWindowHeroView?.Invoke();
        }
        public void HideSelectButton()
        {
            buttonSelectHero.gameObject.SetActive(false);
        }
        public void ClearAllViewSelected()
        {
            foreach (var hero in heroViews)
            {
                hero.ClearSelectWindow();
            }
        }

        public void SetActiveView()
        {
            foreach (var go in BlurObjects)
            {
                go.SetActive(true);
            }
        }
        public void SetInactiveView()
        {
            foreach (var go in BlurObjects)
            {
                go.SetActive(false);
            }
        }
        
        public void ShowTipBanHeroText()
        {
            textSelectHero.text = "Забаньте героя";
        }

        public void ShowTipBanEnemyText()
        {
            textSelectHero.text = "Противник банит героя";
        }
        public void ShowTipPickEnemyText()
        {
            textSelectHero.text = "Противник выберает героя";
        }

        public void ShowTipPickHeroText()
        {
            textSelectHero.text = "Выберите героя";
        }
    }
}