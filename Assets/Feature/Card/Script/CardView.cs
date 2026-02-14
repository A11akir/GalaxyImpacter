using Feature.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Card.Script
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _healthMinion;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _iconSpell;
        [SerializeField] private Image _iconMinion;

        public void SetDataView(CardStatsData cardStatsData)
        {
            gameObject.SetActive(true);
            _name.text = cardStatsData.Name;
            _cost.text = cardStatsData.Cost.ToString();
            _description.text = cardStatsData.Description;
            _iconSpell.sprite = cardStatsData.IconImage;
        }
    }
}