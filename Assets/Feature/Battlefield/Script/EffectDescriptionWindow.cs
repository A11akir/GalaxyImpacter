using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Feature.Battlefield.Script
{
    public class EffectDescriptionWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _nameEffectText;
        [SerializeField] private float _fadeDuration = 0.2f;
        

        public void Show( string nameText, string descriptionText)
        {
            _nameEffectText.text = nameText;
            _descriptionText.text = descriptionText;
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1f, _fadeDuration);
        }

        public void Hide()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0f, _fadeDuration);
        }
    }
}