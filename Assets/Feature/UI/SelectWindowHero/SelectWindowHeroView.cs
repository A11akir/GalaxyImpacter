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
        
        public event Action OnChoseHeroButtonClicked;       
        public event Action OnBanHeroButtonClicked;

        private void OnEnable()
        {
            buttonSelectHero.onClick.AddListener(() => OnChoseHeroButtonClicked?.Invoke());
            buttonBanHero.onClick.AddListener(() => OnBanHeroButtonClicked?.Invoke());
        }
    }
}