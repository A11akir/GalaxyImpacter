// PassiveEffectIconView.cs
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

        public void SetIcon(Sprite sprite) => icon.sprite = sprite;
        public void SetValue(int value) => mainValueText.text = value.ToString();
        public void SetDescription(string text) => descriptionText.text = text;

        public void OnPointerEnter(PointerEventData eventData) => descriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => descriptionWindow.SetActive(false);
    }
}