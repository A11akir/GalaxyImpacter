using Feature.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Card.Script
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _healthMinion;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _iconSpell;
        [SerializeField] private Image _iconMinion;

        [SerializeField] private GameObject _heroCardWindow;
        [SerializeField] private GameObject _spellCardWindow;
        public void SetDataView(CardStatsData cardStatsData)
        {
            gameObject.SetActive(true);
            if (cardStatsData.IsHero)
            {
                _heroCardWindow.SetActive(true);
                _health.text = cardStatsData.Health.ToString();
                _iconMinion.sprite = cardStatsData.IconImage;
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
    }
}