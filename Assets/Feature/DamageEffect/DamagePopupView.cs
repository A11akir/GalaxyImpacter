namespace Feature.DamageEffect
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    public class DamagePopupView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _lifetime = 1f;
        [SerializeField] private float _floatDistance = 60f;
        [SerializeField] private float _punchScale = 0.4f;

        private Vector3 _startPosition;
        private Sequence _sequence;

        private void Awake()
        {
            _startPosition = transform.localPosition;

        }

        public void Show(int amount)
        {
            _damageText.text = amount.ToString();

            _sequence?.Kill();

            transform.localPosition = _startPosition;
            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1f;

            _sequence = DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector3.one * _punchScale, 0.25f, 5, 0.6f))
                .Join(transform.DOLocalMoveY(_startPosition.y + _floatDistance, _lifetime).SetEase(Ease.OutCubic))
                .Insert(_lifetime * 0.5f, _canvasGroup.DOFade(0f, _lifetime * 0.5f));
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }
    }
}