using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.Common;
using Feature.GoogleSheets;
using Feature.PassiveEffect.Script;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Hero.Script
{
    public class HeroPowerPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject heroPowerDescriptionWindow;
        [SerializeField] private TextMeshProUGUI nameHeroPower;
        [SerializeField] private TextMeshProUGUI costHeroPower;
        [SerializeField] private TextMeshProUGUI descriptionHeroPower;
        [SerializeField] private List<Image> iconHeroPower;
        [SerializeField] protected GameObject _costFrame;

        private string _baseDescription; // ← хранит исходное описание силы героя

        public void OnPointerEnter(PointerEventData eventData) => heroPowerDescriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => heroPowerDescriptionWindow.SetActive(false);

        public void SetDataView(CardStatsData cardStatsData)
        {
            var spell = (SpellCardData)cardStatsData;
            nameHeroPower.text = spell.Name;
            costHeroPower.text = spell.Cost.ToString();

            _baseDescription = spell.Description;
            descriptionHeroPower.text = _baseDescription;

            GLog.Log(_baseDescription);
            foreach (var sprite in iconHeroPower)
                sprite.sprite = spell.IconImage;

            if (spell.IsPassive)
                SetPassiveView();
        }

        public virtual void SetPassiveView()
        {
            if (_costFrame != null)
                _costFrame.SetActive(false);
        }

        public void SetPassiveEffectData(PassiveEffectBase passive)
        {
            int value = (passive is IValueProvider provider) ? provider.Value.CurrentValue : 0;
            string desc = passive.GetDescription(value);
            descriptionHeroPower.text = desc;
        }

        public void ClearPassiveEffectData()
        {
            descriptionHeroPower.text = ""; 
        }
        
    }
}