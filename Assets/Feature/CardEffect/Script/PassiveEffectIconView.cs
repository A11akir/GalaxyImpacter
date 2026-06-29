using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.CardEffect.Script
{
    public class PassiveEffectIconView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _descriptionCanvasGroup;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI mainValueText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private float _pulseDuration = 1f;
        [SerializeField] private float _pulseScale = 1.2f;

        public bool IsInUse { get; private set; }

        private bool _isHovered;

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
            IsInUse = true;
            _canvasGroup.alpha = 0f;
            _descriptionCanvasGroup.alpha = 0f;
        }

        public void SetValue(int? value)
        {
            mainValueText.text = value.HasValue ? value.Value.ToString() : "";
            PlayPulse();
        }

        public void HideValue()
        {
            mainValueText.text = "";
        }
        
        
        public void SetDescription(string text) => descriptionText.text = text;

        public void SetHoverState(bool hovered)
        {
            if (!IsInUse) return;

            _isHovered = hovered;

            if (hovered)
            {
                _canvasGroup.DOKill();
                _canvasGroup.alpha = 1f;

                _descriptionCanvasGroup.DOKill();
                _descriptionCanvasGroup.alpha = 1f;
            }
            else
            {
                _descriptionCanvasGroup.DOKill();
                _descriptionCanvasGroup.alpha = 0f;

                if (!IsPulseActive())
                    _canvasGroup.DOFade(0f, 0.3f);
            }
        }

        public void ForceHide()
        {
            IsInUse = false;

            _canvasGroup.DOKill();
            _descriptionCanvasGroup.DOKill();
            transform.DOKill();

            _canvasGroup.alpha = 0f;
            _descriptionCanvasGroup.alpha = 0f;

            HideValue();

            transform.localScale = Vector3.one;
        }

        private bool IsPulseActive() => DOTween.IsTweening(transform);

        public void PlayPulse()
        {
            transform.DOKill();
            _canvasGroup.DOKill();

            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1f;

            transform.DOScale(_pulseScale, _pulseDuration * 0.2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                    transform.DOScale(Vector3.one, _pulseDuration * 0.8f)
                        .SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            if (!_isHovered)
                                _canvasGroup.DOFade(0f, 0.3f);
                        }));
        }
    }
}