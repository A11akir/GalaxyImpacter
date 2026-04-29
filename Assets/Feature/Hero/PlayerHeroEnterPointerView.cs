using Feature.Items.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Hero
{
    public class PlayerHeroEnterPointerView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InventoryView _inventoryView;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _inventoryView.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inventoryView.Hide();
        }
    }
}