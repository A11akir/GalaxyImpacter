using TMPro;
using UnityEngine;

namespace Feature.ShopGamePlay.Script
{
    public class CurrencyManageView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyCountText;

        public void SetCurrencyText(int currencyAmount)
        {
            if (_currencyCountText != null)
            {
                _currencyCountText.text = currencyAmount.ToString();
            }
        }
    }
}