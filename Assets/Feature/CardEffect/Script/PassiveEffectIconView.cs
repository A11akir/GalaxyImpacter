// PassiveEffectIconView.cs
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.CardEffect.Script
{
    public class PassiveEffectIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI mainValueText;
        [SerializeField] private GameObject descriptionWindow;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private float _pulseDuration = 1f;
        [SerializeField] private float _pulseScale = 1.2f;

        public void SetIcon(Sprite sprite) => icon.sprite = sprite;

        public void SetValue(int value)
        {
            mainValueText.text = value.ToString();
            PlayPulse();
        }

        public void SetDescription(string text) => descriptionText.text = text;

        public void OnPointerEnter(PointerEventData eventData) => descriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => descriptionWindow.SetActive(false);

private void PlayPulse()
{
    transform.DOKill();
    transform.localScale = Vector3.one;

    gameObject.SetActive(true);

    transform.DOScale(_pulseScale, _pulseDuration * 0.2f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
            transform.DOScale(Vector3.one, _pulseDuration * 0.8f)
                .SetEase(Ease.InQuad)
                .OnComplete(() => gameObject.SetActive(false)));
}
    }
}