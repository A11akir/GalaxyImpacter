
using Feature.ClassBranchWindow.Script;
using Feature.Items.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Hero
{
    public class PlayerHeroEnterPointerView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private ClassLevelWindowView classLevelWindowView; 

        public void OnPointerEnter(PointerEventData eventData)
        {
            inventoryView?.Show();
            classLevelWindowView?.Show(); 
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryView?.Hide();
            classLevelWindowView?.Hide(); 
        }
    }
}