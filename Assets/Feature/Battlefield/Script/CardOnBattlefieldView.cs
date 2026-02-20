using Feature.Card.Script;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Feature.Battlefield.Script
{
    public class CardOnBattlefieldView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _healthBoard;
        [SerializeField] private TextMeshProUGUI _cost;
        
        [SerializeField] private Image _iconMinionHand;
        [SerializeField] private Image _iconMinionBoard;

        [SerializeField] private GameObject _borderHasAction; 
        [SerializeField] private GameObject _heroDescriptionWindow;

        public void SetDataView(CardStatsData cardStatsData)
        {
            gameObject.SetActive(true);
            _heroDescriptionWindow.SetActive(false);
            
            _health.text = cardStatsData.Health.ToString();
            _healthBoard.text = cardStatsData.Health.ToString();
            _iconMinionHand.sprite = cardStatsData.IconImage;
            _iconMinionBoard.sprite = cardStatsData.IconImage;
            _name.text = cardStatsData.Name;
            _cost.text = cardStatsData.Cost.ToString();
        }

        public void SetCanHasAction(bool canCast) => _borderHasAction.SetActive(canCast);
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _heroDescriptionWindow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _heroDescriptionWindow.SetActive(false);
        }
    }
}