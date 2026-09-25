using DG.Tweening;
using Feature.Card.Script;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Battlefield.Script
{
    public class EffectHeroOnBoardView : EffectIconViewBase, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private EffectDescriptionWindow _descriptionWindow;
        [SerializeField] private float _hoverScale = 1.15f;
        [SerializeField] private float _hoverDuration = 0.2f;
        [SerializeField] private float _pulseDuration = 1f;
        [SerializeField] private float _pulseScale = 1.2f;

        private Vector3 _defaultScale;
        private bool _isBoardHovered;

        private void Awake() => _defaultScale = transform.localScale;

        public override void SetIcon(Sprite sprite)
        {
            base.SetIcon(sprite);
            _canvasGroup.alpha = 0f;
        }

        public override void SetValue(int? value)
        {
            base.SetValue(value);
            PlayPulse();
        }

        // вызывается контейнером при наведении на существо целиком
        public void SetBoardHoverState(bool hovered)
        {
            if (!IsInUse) return;

            _isBoardHovered = hovered;
            _canvasGroup.DOKill();

            if (hovered)
                _canvasGroup.alpha = 1f;
            else if (!IsPulseActive())
                _canvasGroup.DOFade(0f, 0.3f);
        }

        public override void ForceHide()
        {
            IsInUse = false;
            _canvasGroup.DOKill();
            transform.DOKill();
            _canvasGroup.alpha = 0f;
            HideValue();
            transform.localScale = _defaultScale;
            _descriptionWindow.Hide();
        }

        // наведение на саму иконку — показывает описание
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInUse) return;
            transform.DOKill();
            transform.DOScale(_defaultScale * _hoverScale, _hoverDuration).SetEase(Ease.OutBack);
            _descriptionWindow.Show(_nameEffect, _description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsInUse) return;
            transform.DOKill();
            transform.DOScale(_defaultScale, _hoverDuration).SetEase(Ease.InQuad);
            _descriptionWindow.Hide();
        }

        private bool IsPulseActive() => DOTween.IsTweening(transform);

        public void PlayPulse()
        {
            transform.DOKill();
            _canvasGroup.DOKill();

            transform.localScale = _defaultScale;
            _canvasGroup.alpha = 1f;

            transform.DOScale(_defaultScale * _pulseScale, _pulseDuration * 0.2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                    transform.DOScale(_defaultScale, _pulseDuration * 0.8f)
                        .SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            if (!_isBoardHovered)
                                _canvasGroup.DOFade(0f, 0.3f);
                        }));
        }
    }
}