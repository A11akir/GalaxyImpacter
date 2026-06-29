using System;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.Entity.Script;
using Feature.GoogleSheets;
using Feature.Health;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ITargetable = Feature.GameSessionData.ITargetable;

namespace Feature.Battlefield.Script.View
{
    public class CardOnBattlefieldView : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ITargetable, IEntityView
    {
        [SerializeField] private TextMeshProUGUI _name;
        
        [SerializeField] private GameObject _armorRoot;
        [SerializeField] private TextMeshProUGUI _armorText;
        [SerializeField] private TextMeshProUGUI _health;
        
        [SerializeField] private TextMeshProUGUI _healthBoard;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Image _iconMinionHand;
        [SerializeField] private Image _iconMinionBoard;
        [SerializeField] private GameObject _borderHasAction;
        [SerializeField] private GameObject _heroDescriptionWindow;
        [SerializeField] private GameObject _selectEntityView;

        [Header("Rarity Sprites")] [SerializeField]
        private GameObject commonSprite;

        [SerializeField] private GameObject hiddenSprite;
        [SerializeField] private GameObject anomalousSprite;
        [SerializeField] private GameObject primordialSprite;

        
        [SerializeField] private PassiveEffectsContainerView _passiveEffectsView;
        public PassiveEffectsContainerView PassiveEffectsView => _passiveEffectsView;

        
        public event Action OnClicked;

        public void SetDataView(MinionCardData cardStatsData)
        {
            gameObject.SetActive(true);
            _heroDescriptionWindow.SetActive(false);
            _health.text = cardStatsData.Health.ToString();
            _healthBoard.text = cardStatsData.Health.ToString();
            _iconMinionHand.sprite = cardStatsData.IconImage;
            _iconMinionBoard.sprite = cardStatsData.IconImage;
            _name.text = cardStatsData.Name;
            _cost.text = cardStatsData.Cost.ToString();

            SetRaritySprite(cardStatsData.Rarity);
        }
        
        public void SetArmor(int armor)
        {
            bool hasArmor = armor > 0;

            _armorRoot.SetActive(hasArmor);

            _armorText.text = hasArmor 
                ? armor.ToString() 
                : "";
        }
        
        private void SetRaritySprite(CardRarity rarity)
        {
            commonSprite.SetActive(rarity == CardRarity.Common);
            hiddenSprite.SetActive(rarity == CardRarity.Hidden);
            anomalousSprite.SetActive(rarity == CardRarity.Anomalous);
            primordialSprite.SetActive(rarity == CardRarity.Primordial);
        }

        public void SetCanHasAction(bool canCast) => _borderHasAction.SetActive(canCast);
        public void SetSelected(bool selected) => _selectEntityView.SetActive(selected);

        public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke();

        public void OnPointerEnter(PointerEventData eventData) => _heroDescriptionWindow.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => _heroDescriptionWindow.SetActive(false);

        public void SetHealth(int hp)
        {
            _health.text = hp.ToString();
            _healthBoard.text = hp.ToString();
        }

        public void ClearData()
        {
            _name.text = "";
            _health.text = "";
            _healthBoard.text = "";
            _cost.text = "";
            _iconMinionHand.sprite = null;
            _iconMinionBoard.sprite = null;
            _heroDescriptionWindow.SetActive(false);
            _selectEntityView.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}