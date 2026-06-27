using System;
using Feature.GoogleSheets;
using R3;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Card.Script
{
    public class HandCardView : MonoBehaviour
    {
        [SerializeField] public GameObject _cardContainer;
        [SerializeField] public GameObject _healthContainer;
        [SerializeField] public TextMeshProUGUI _nameSpell;
        [SerializeField] public TextMeshProUGUI _nameMinion;
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] protected TextMeshProUGUI _cost;
        [SerializeField] protected TextMeshProUGUI _description;
        [SerializeField] protected Image _iconSpell;
        [SerializeField] private Image _iconMinionHand;

        [SerializeField] private GameObject _canAvailableCast;
        [SerializeField] private GameObject _heroCardWindow;
        [SerializeField] private GameObject _spellCardWindow;
        [SerializeField] private GameObject _cardBack;
        [SerializeField] private GameObject _costFrame;

        [Header("Rarity Sprites")]
        [SerializeField] private GameObject commonSprite;
        [SerializeField] private GameObject hiddenSprite;
        [SerializeField] private GameObject anomalousSprite;
        [SerializeField] private GameObject primordialSprite;
        
        private readonly CompositeDisposable _passiveSubscriptions = new();
        
        public  virtual void SetDataView(CardStatsData cardStatsData)
        {
            if (_cardBack) _cardBack.SetActive(false);
            gameObject.SetActive(true);

            _heroCardWindow.SetActive(false);
            _spellCardWindow.SetActive(false);
            _healthContainer.SetActive(false);
            _description.gameObject.SetActive(false);

            if (cardStatsData is MinionCardData minion)
            {
                _nameMinion.text = cardStatsData.Name;
                _healthContainer.SetActive(true);
                _heroCardWindow.SetActive(true);
                _health.text = minion.Health.ToString();
                _iconMinionHand.sprite = minion.IconImage;
            }
            else if (cardStatsData is SpellCardData spell)
            {
                _nameSpell.text = cardStatsData.Name;
                _description.gameObject.SetActive(true);
                _spellCardWindow.SetActive(true);
                _description.text = spell.Description;
                _iconSpell.sprite = spell.IconImage;
            }

            
            SetCost(cardStatsData.Cost);
    
            SetRaritySprite(cardStatsData.Rarity);
        }

        public void SetCost(int cost) => _cost.text = cost.ToString();
        
        private void SetRaritySprite(CardRarity rarity)
        {
            commonSprite.SetActive(rarity == CardRarity.Common);
            hiddenSprite.SetActive(rarity == CardRarity.Hidden);
            anomalousSprite.SetActive(rarity == CardRarity.Anomalous);
            primordialSprite.SetActive(rarity == CardRarity.Primordial);
        }
        public void ClearData()
        {
            _passiveSubscriptions.Clear();
            _nameSpell.text = "";
            _nameMinion.text = "";
            _health.text = "";
            _cost.text = "";
            _description.text = "";
            _iconMinionHand.sprite = null;
            _iconSpell.sprite = null;
            _heroCardWindow.SetActive(false);
            _spellCardWindow.SetActive(false);
            _canAvailableCast.SetActive(false);
        }
        
        public void ShowAsHidden()
        {
            gameObject.SetActive(true);
            _heroCardWindow.SetActive(false);
            _spellCardWindow.SetActive(false);
            _canAvailableCast.SetActive(false);
            _healthContainer.SetActive(false);
            _costFrame.SetActive(false);
            _cost.gameObject.SetActive(false);
            _cardBack.SetActive(true);
        }

        public void ShowAsOpen()
        {
            _cardBack.SetActive(false);
        }
        
        public void SetPassiveSubscriptions(IDisposable subscriptions)
        {
            _passiveSubscriptions.Clear();
            _passiveSubscriptions.Add(subscriptions);
        }
        
        public void SetCanCastView(bool canCast) => _canAvailableCast.SetActive(canCast);
    }
}