using System.Collections.Generic;
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
    
        [SerializeField] private List<int> values;
    
        [SerializeField] private GameObject descriptionWindow;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetData(PassiveEffect.Script.PassiveEffect passive)
        {
            icon = passive.Icon;
            descriptionText.text = passive.Description;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            descriptionWindow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            descriptionWindow.SetActive(false);
        }
    }
}