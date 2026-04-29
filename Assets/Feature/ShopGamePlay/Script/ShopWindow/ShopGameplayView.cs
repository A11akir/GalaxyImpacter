using System.Collections.Generic;
using DG.Tweening;
using Feature.Items.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplayView : MonoBehaviour
    {
        [SerializeField] private Button _hideShopWindowButton;
        [SerializeField] private Button _showShopWindowButton;
        [SerializeField] List<ItemShopView> itemsView = new List<ItemShopView>();

        
        private RectTransform _rectTransform;
        private bool _isLocked;

        private const float VisibleRight = 450f;
        private const float HiddenRight = -1035f;
        private const float AnimDuration = 0.6f;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetRight(HiddenRight);
        }
        
        private void OnEnable()
        {
            _hideShopWindowButton.onClick.AddListener(HideShopWindow);
            _showShopWindowButton.onClick.AddListener(ShowShopWindow);
            
            _showShopWindowButton.gameObject.SetActive(true);
            _hideShopWindowButton.gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            _hideShopWindowButton.onClick.RemoveListener(HideShopWindow);
            _showShopWindowButton.onClick.RemoveListener(ShowShopWindow);
        }

        public void UnlockShop()
        {
            _isLocked = false;
            _showShopWindowButton.gameObject.SetActive(true);
        }

        public void LockShop()
        {
            _isLocked = true;
            HideShopWindow();
            _showShopWindowButton.gameObject.SetActive(false);
            _hideShopWindowButton.gameObject.SetActive(false);
        }
        
        private void ShowShopWindow()
        {
            if (_isLocked) return;
            _showShopWindowButton.gameObject.SetActive(false);
            _hideShopWindowButton.gameObject.SetActive(true);
            AnimateRight(VisibleRight, Ease.Linear);
        }

        private void HideShopWindow()
        {
            if (!_isLocked)
                _showShopWindowButton.gameObject.SetActive(true);
            _hideShopWindowButton.gameObject.SetActive(false);
            AnimateRight(HiddenRight, Ease.Linear);
        }

        private void AnimateRight(float targetRight, Ease ease)
        {
            if (!_rectTransform) return;
    
            DOTween.To(
                    () => -_rectTransform.offsetMax.x,  // ← минус
                    x => _rectTransform.offsetMax = new Vector2(-x, _rectTransform.offsetMax.y),
                    targetRight,
                    AnimDuration)
                .SetEase(ease);
        }

        private void SetRight(float right)
        {
            var offsetMax = _rectTransform.offsetMax;
            _rectTransform.offsetMax = new Vector2(-right, offsetMax.y);
        }


        
        public void RefreshViewShop(List<ItemData> gameDataAllItems)
        {
            for (int i = 0; i < itemsView.Count; i++)
            {
                itemsView[i].SetView(gameDataAllItems[i]);
            }
        }
    }
}