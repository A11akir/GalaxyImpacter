using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Items.Scripts
{
    public class ItemShopView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _goldCost;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _iconImage;
    
        [SerializeField] private GameObject _descriptionWindow;
        [SerializeField] private Transform _containerForDescription;
        private Transform _nativeContainerForDescription;

        public void SetView(ItemData itemData)
        {
            _goldCost.text = itemData.GoldCost.ToString();
            _name.text = itemData.ItemName;
            _description.text = itemData.Description;
            _iconImage.sprite = itemData.IconImage;
            
            _nativeContainerForDescription = _containerForDescription.parent;
            _descriptionWindow.SetActive(false);
            
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _descriptionWindow.SetActive(true);
            AdjustPositionIfOutOfBounds();
            _descriptionWindow.transform.SetParent(_containerForDescription);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _descriptionWindow.SetActive(false);
            _descriptionWindow.transform.SetParent(_nativeContainerForDescription);
        }

        private void AdjustPositionIfOutOfBounds()
        {
            RectTransform rectTransform = _descriptionWindow.GetComponent<RectTransform>();
        
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        
            Vector3[] windowCorners = new Vector3[4];
            rectTransform.GetWorldCorners(windowCorners);
        
            Vector3[] canvasCorners = new Vector3[4];
            canvasRect.GetWorldCorners(canvasCorners);
        
            bool outOfBoundsLeft = windowCorners[0].x < canvasCorners[0].x;
            bool outOfBoundsRight = windowCorners[2].x > canvasCorners[2].x;
        
        
            if (outOfBoundsLeft || outOfBoundsRight)
            {
                Vector3 currentPos = rectTransform.anchoredPosition;
                rectTransform.anchoredPosition = new Vector3(-currentPos.x, currentPos.y, currentPos.z);
            }

        }
    }
}