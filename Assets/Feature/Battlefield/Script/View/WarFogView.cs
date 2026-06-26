using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Feature.Battlefield.Script.View
{
    public class WarFogView : MonoBehaviour
    {
        [SerializeField] private GameObject fogWarView;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            _rectTransform = fogWarView.GetComponent<RectTransform>();
        }

        public void ShowFog()
        {
            fogWarView.SetActive(true);
            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, 100f);
            
            DOTween.Sequence()
                .Join(_rectTransform.DOAnchorPosY(-115f, 1f).SetEase(Ease.OutQuad))
                .Join(_canvasGroup.DOFade(1f, 1f));
            
            _canvasGroup.blocksRaycasts = true;
        }

        public void HideFog()
        {
            DOTween.Sequence()
                .Join(_rectTransform.DOAnchorPosY(100f, 1f).SetEase(Ease.InQuad))
                .Join(_canvasGroup.DOFade(0f, 1f))
                .OnComplete(() => fogWarView.SetActive(false));
            
            _canvasGroup.blocksRaycasts = false;
        }
        
        
        [Button("Toggle Fog (Debug)")]
        private void ToggleFogDebug()
        {
            if (fogWarView.activeSelf)
                HideFog();
            else
                ShowFog();
        }
    }
}