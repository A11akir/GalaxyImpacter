using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class ShopGameplayView : MonoBehaviour
    {
        [SerializeField] private Button _hideShopWindowButton;
        [SerializeField] private Button _showShopWindowButton;
        
        private RectTransform _rectTransform;
        private Vector2 _hiddenPosition;
        private Vector2 _visiblePosition;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            _visiblePosition = new Vector2(-100, 0);
            _hiddenPosition = new Vector2(-1600, 0);
            
            _rectTransform.anchoredPosition = _hiddenPosition;
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
        
        private void ShowShopWindow()
        {
            if (_rectTransform == null) return;
            
            _rectTransform.DOAnchorPos(_visiblePosition, 0.5f)
                .SetEase(Ease.OutBack)
                .OnStart(() => 
                {
                    _showShopWindowButton.gameObject.SetActive(false);
                    _hideShopWindowButton.gameObject.SetActive(true);
                });
        }
        
        private void HideShopWindow()
        {
            if (_rectTransform == null) return;
            
            _rectTransform.DOAnchorPos(_hiddenPosition, 0.5f)
                .SetEase(Ease.InBack)
                .OnStart(() => 
                {
                    _showShopWindowButton.gameObject.SetActive(true);
                    _hideShopWindowButton.gameObject.SetActive(false);
                });
        }
    }
}