using System.Collections.Generic;
using Feature.GoogleSheets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Hero
{
    public class HeroPowerPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject heroPowerDescriptionWindow;
        [SerializeField]private TextMeshProUGUI nameHeroPower;
        [SerializeField]private TextMeshProUGUI costHeroPower;
        [SerializeField] private TextMeshProUGUI descriptionHeroPower;
        [SerializeField] private List<Image> iconHeroPower;
        public void OnPointerEnter(PointerEventData eventData) => heroPowerDescriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => heroPowerDescriptionWindow.SetActive(false);
        
        public void SetDataView(CardStatsData cardStatsData)
        {
            var spell = (SpellCardData)cardStatsData;
            nameHeroPower.text = spell.Name;
            costHeroPower.text = spell.Cost.ToString();
            descriptionHeroPower.text = spell.Description;

            foreach (var sprite in iconHeroPower)
                sprite.sprite = spell.IconImage;
        }
    }
}