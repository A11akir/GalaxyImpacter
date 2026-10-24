using TMPro;
using UnityEngine;

namespace Feature.Shield.Script
{
    /// <summary>
    /// Вьюшка щита: есть щит (value > 0) — показать объект и число, нет — скрыть.
    /// </summary>
    public class ShieldView : MonoBehaviour
    {
        [SerializeField] private GameObject _shieldContainer;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void SetShield(int value)
        {
            bool hasShield = value > 0;

            _shieldContainer.SetActive(hasShield);

            _valueText.text = value.ToString();
        }
    }
}