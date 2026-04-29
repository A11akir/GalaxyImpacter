using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Feature.Items.Scripts
{
    public class ItemShopView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _goldCost;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _iconImage;
    
        [SerializeField] private GameObject _descriptionWindow;
        [SerializeField] private Transform _containerForDescription;
        private Transform _nativeContainerForDescription;

        private ItemData _itemData;
        public System.Action<ItemData> OnItemClicked;

        private void Awake()
        {
            if (_descriptionWindow != null)
            {
                _nativeContainerForDescription = _descriptionWindow.transform.parent;
                _descriptionWindow.SetActive(false);
            }
        }

        public void SetView(ItemData itemData)
        {
            _itemData = itemData;
            _goldCost.text = itemData.GoldCost.ToString();
            _name.text = itemData.ItemName;
            _description.text = itemData.Description;
            _iconImage.sprite = itemData.IconImage;
            
            
            if (_descriptionWindow != null && _nativeContainerForDescription != null)
            {
                _descriptionWindow.transform.SetParent(_nativeContainerForDescription);
                _descriptionWindow.SetActive(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_descriptionWindow == null || _containerForDescription == null) return;
            
            _descriptionWindow.SetActive(true);
            _containerForDescription.position = transform.position;
            _descriptionWindow.transform.SetParent(_containerForDescription);
            AdjustPositionIfOutOfBounds();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_descriptionWindow == null || _nativeContainerForDescription == null) return;
            
            _descriptionWindow.transform.SetParent(_nativeContainerForDescription);
            _descriptionWindow.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked?.Invoke(_itemData);
        }
        
        private void OnDisable()
        {
            if (_descriptionWindow != null && _nativeContainerForDescription != null)
            {
                _descriptionWindow.transform.SetParent(_nativeContainerForDescription);
                _descriptionWindow.SetActive(false);
            }
        }

        public void PlayPurchaseAnimation()
        {
            var iconTransform = _iconImage.transform;
            var originalScale = iconTransform.localScale;
            
            var sequence = DOTween.Sequence();
            
            sequence.Append(iconTransform.DOScale(originalScale * 1.2f, 0.15f));
            sequence.Append(iconTransform.DOScale(originalScale * 0.9f, 0.1f));
            sequence.Append(iconTransform.DOScale(originalScale, 0.1f));
            
            sequence.Join(iconTransform.DORotate(new Vector3(0, 0, 10f), 0.2f).SetLoops(2, LoopType.Yoyo));
            
            sequence.Play();
        }

        public void PlayCannotAffordAnimation()
        {
            var iconTransform = _iconImage.transform;
            var originalColor = _iconImage.color;
            
            var sequence = DOTween.Sequence();
            
            sequence.Append(iconTransform.DOShakePosition(0.3f, strength: 10f, vibrato: 20));
            sequence.Join(_iconImage.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo));
            
            sequence.OnComplete(() => _iconImage.color = originalColor);
            
            sequence.Play();
        }

        private void AdjustPositionIfOutOfBounds()
        {
            RectTransform rectTransform = _descriptionWindow.GetComponent<RectTransform>();

            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            
            rectTransform.anchoredPosition = new Vector2(-225f, rectTransform.anchoredPosition.y);
            
            Canvas.ForceUpdateCanvases();
            
            Vector3[] windowCorners = new Vector3[4];
            rectTransform.GetWorldCorners(windowCorners);

            Vector3[] canvasCorners = new Vector3[4];
            canvasRect.GetWorldCorners(canvasCorners);

            bool outOfBoundsLeft = windowCorners[0].x < canvasCorners[0].x;
            bool outOfBoundsRight = windowCorners[2].x > canvasCorners[2].x;
            
            if (outOfBoundsLeft)
            {
                rectTransform.anchoredPosition = new Vector2(225f, rectTransform.anchoredPosition.y);
            }
            else if (outOfBoundsRight)
            {
                rectTransform.anchoredPosition = new Vector2(-225f, rectTransform.anchoredPosition.y);
            }
        }
    }
}