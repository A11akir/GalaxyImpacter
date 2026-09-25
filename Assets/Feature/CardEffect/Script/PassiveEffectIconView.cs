using DG.Tweening;
using Feature.Card.Script;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.CardEffect.Script
{
    public class PassiveEffectIconView : EffectIconViewBase, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _descriptionCanvasGroup;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [SerializeField] private float _pulseDuration = 1f;
        [SerializeField] private float _pulseScale = 1.2f;

        private bool _isHovered;

        public override void SetIcon(Sprite sprite)
        {
            base.SetIcon(sprite);
            _canvasGroup.alpha = 0f;
            _descriptionCanvasGroup.alpha = 0f;
        }

        public override void SetValue(int? value)
        {
            base.SetValue(value);
            PlayPulse();
        }

        public override void SetDescription(string text)  // ← добавили сюда
        {
            base.SetDescription(text);
            _descriptionText.text = text;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInUse) return;
            _descriptionCanvasGroup.DOKill();
            _descriptionCanvasGroup.alpha = 1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsInUse) return;
            _descriptionCanvasGroup.DOKill();
            _descriptionCanvasGroup.alpha = 0f;
        }
        
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

        public override void ForceHide()
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