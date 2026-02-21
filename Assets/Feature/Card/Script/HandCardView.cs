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
        [SerializeField] public TextMeshProUGUI _name;
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

        public void ClearData()
        {
            _name.text = "";
            _health.text = "";
            _cost.text = "";
            _description.text = "";
            _iconMinionHand.sprite = null;
            _iconSpell.sprite = null;
            _heroCardWindow.SetActive(false);
            _spellCardWindow.SetActive(false);
            _canAvailableCast.SetActive(false);
        }
        
        public void SetCanCastView(bool canCast) => _canAvailableCast.SetActive(canCast);

        public void ViewDelete()
        {
            Debug.Log("ViewDelete");
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
        }
    }
}