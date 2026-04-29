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

        private const float VisibleX = -515f;
        private const float HiddenX = -2000f;
        private const float AnimDuration = 0.6f;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetPositionX(HiddenX);
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
            AnimatePositionX(VisibleX, Ease.OutBack);
        }

        private void HideShopWindow()
        {
            if (!_isLocked)
                _showShopWindowButton.gameObject.SetActive(true);
            _hideShopWindowButton.gameObject.SetActive(false);
            AnimatePositionX(HiddenX, Ease.InBack);
        }

        private void AnimatePositionX(float targetX, Ease ease)
        {
            if (!_rectTransform) return;
    
            DOTween.To(
                    () => _rectTransform.anchoredPosition.x, 
                    x => _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y),
                    targetX,
                    AnimDuration)
                .SetEase(ease);
        }

        private void SetPositionX(float x)
        {
            _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y);
        }
        
        
        public List<ItemShopView> GetItemViews()
        {
            return itemsView;
        }
    }
}