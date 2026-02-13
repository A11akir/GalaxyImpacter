using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroView : MonoBehaviour
    {
        [SerializeField] public List<HeroView> heroViews;
        [SerializeField] public Button buttonSelectHero;
        [SerializeField] public Button buttonBanHero;

        [HideInInspector]public HeroView _selectHeroView;
        public event Action OnSelectWindowHeroView;
        public event Action OnChoseHeroButtonClicked;       
        public event Action OnBanHeroButtonClicked;

        private void OnEnable()
        {
            buttonSelectHero.onClick.AddListener(() => OnChoseHeroButtonClicked?.Invoke());
            buttonBanHero.onClick.AddListener(() => OnBanHeroButtonClicked?.Invoke());

            foreach (var heroView in heroViews)
            {
                heroView.OnSelectHeroView += HeroViewViewOnOnSelectHeroView;
            }
        }

        private void HeroViewViewOnOnSelectHeroView(HeroView heroView)
        {
            _selectHeroView = heroView;
            ClearAllSelected();
            
            OnSelectWindowHeroView?.Invoke();
        }

        public void ClearAllSelected()
        {
            foreach (var hero in heroViews)
            {
                hero.ClearSelectWindow();
            }
        }
    }
}