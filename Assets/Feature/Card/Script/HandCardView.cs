using Feature.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Feature.Card.Script
{
    public class HandCardView : MonoBehaviour
    {
        [SerializeField] public GameObject _cursorArrowLine;
        [SerializeField] public GameObject _cursorArrowHead;
        [SerializeField] public GameObject _cardContainer;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _iconSpell;
        [SerializeField] private Image _iconMinionHand;

        [SerializeField] private GameObject _canAvailableCast;
        [SerializeField] private GameObject _heroCardWindow;
        [SerializeField] private GameObject _spellCardWindow;

        public void SetDataView(CardStatsData cardStatsData)
        {
            gameObject.SetActive(true);
            if (cardStatsData.IsHero)
            {
                _heroCardWindow.SetActive(true);
                _health.text = cardStatsData.Health.ToString();
                _iconMinionHand.sprite = cardStatsData.IconImage;
            }
            else
            {
                _spellCardWindow.SetActive(true);
                _description.text = cardStatsData.Description;
                _iconSpell.sprite = cardStatsData.IconImage;
            }
            
            _name.text = cardStatsData.Name;
            _cost.text = cardStatsData.Cost.ToString();
            
        }

        public void ClearCardData(HandCardView card)
        {
            card._name.text = "";
            card._health.text = "";
            card._cost.text = "";
            card._description.text = "";
            card._iconMinionHand.sprite = null;
            card._iconSpell.sprite = null;
            card._heroCardWindow.SetActive(false);
            card._spellCardWindow.SetActive(false);
            card._canAvailableCast.SetActive(false);
        }
        
        public void SetCanCastView(bool canCast) => _canAvailableCast.SetActive(canCast);
    }
}