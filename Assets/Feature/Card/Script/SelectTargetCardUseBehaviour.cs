using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Card.Script
{
    public class SelectTargetCardUseBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler
    { 
        public GameObject cursorArrowHead;
        public GameObject cursorArrowLine;
        public GameObject cardObject;
        
        private RectTransform _lineRectTransform;
        private RectTransform _headRectTransform;
        private RectTransform _cardRectTransform;
        private Vector2 _startPosition;
        private bool _isDragging = false;
        
        private float _headRotationOffset = -90f;
        
        private void Awake()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            if (cursorArrowLine != null)
                _lineRectTransform = cursorArrowLine.GetComponent<RectTransform>();
                
            if (cursorArrowHead != null)
                _headRectTransform = cursorArrowHead.GetComponent<RectTransform>();
                
            if (cardObject != null)
                _cardRectTransform = cardObject.GetComponent<RectTransform>();
        }
        
        private void Start()
        {
            if (_lineRectTransform == null || _headRectTransform == null || _cardRectTransform == null)
            {
                InitializeComponents();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            cursorArrowLine.SetActive(true);
            cursorArrowHead.SetActive(true);
            cardObject.SetActive(false);
            
            if (!_isDragging)
            {
                _startPosition = _cardRectTransform.position;
                _isDragging = true;
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _cardRectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );
            _cardRectTransform.localPosition = localPoint;
            
            UpdateCursorArrow(eventData);
        }
        
        private void UpdateCursorArrow(PointerEventData eventData)
        {
            if (_cardRectTransform.parent == null) return;
    
            RectTransform parentRect = _cardRectTransform.parent as RectTransform;
            if (parentRect == null) return;
    
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentLocalPoint
            );
    
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                _startPosition,
                eventData.pressEventCamera,
                out Vector2 startLocalPoint
            );
    
            Vector2 direction = currentLocalPoint - startLocalPoint;
            float distance = direction.magnitude;
    
            if (distance > 0.01f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                
                _lineRectTransform.localPosition = startLocalPoint + direction * 0.5f;
                _lineRectTransform.rotation = Quaternion.Euler(0, 0, angle);
                
                Vector2 currentSize = _lineRectTransform.sizeDelta;
                _lineRectTransform.sizeDelta = new Vector2(distance, currentSize.y);
                
                _headRectTransform.localPosition = currentLocalPoint;
                _headRectTransform.rotation = Quaternion.Euler(0, 0, angle + _headRotationOffset);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cursorArrowLine.SetActive(false);
            cursorArrowHead.SetActive(false);
            cardObject.SetActive(true);
            _isDragging = false;
        }
        
        private void OnDisable()
        {
            _isDragging = false;
        }
    }
}