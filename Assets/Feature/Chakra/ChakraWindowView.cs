using TMPro;
using UnityEngine;

namespace Feature.Chakra
{
    public class ChakraWindowView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _chakraCountText;
        public void SetChakraText(int chakra)
        {
            if (_chakraCountText != null)
            {
                _chakraCountText.text = chakra.ToString();
            }
        }
    }
}
