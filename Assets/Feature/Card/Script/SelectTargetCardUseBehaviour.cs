using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Card.Script
{
    public class SelectTargetCardUseBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        public GameObject cursorArrowHead;
        public GameObject cursorArrowLine;
        public GameObject cardObject;

        private RectTransform _lineRectTransform;
        private RectTransform _headRectTransform;
        private RectTransform _cardRectTransform;
        
        private Vector2 _startPosition;
        
        private bool _isDragging;

        private int _headRotationOffset = 180;

        public void Init()
        {
            _lineRectTransform = cursorArrowLine.GetComponent<RectTransform>();
            _headRectTransform = cursorArrowHead.GetComponent<RectTransform>();
            _cardRectTransform = cardObject.GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData) => UpdateCursorArrow(eventData);

        private void UpdateCursorArrow(PointerEventData eventData)
        {
            RectTransform parentRect = _cardRectTransform.parent as RectTransform;

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

                _lineRectTransform.sizeDelta = new Vector2(distance, _lineRectTransform.sizeDelta.y);

                _headRectTransform.localPosition = currentLocalPoint;
                _headRectTransform.rotation = Quaternion.Euler(0, 0, angle + 180);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cursorArrowLine.SetActive(false);
            cursorArrowHead.SetActive(false);
            cardObject.SetActive(true);
            _isDragging = false;
        }

        private void OnDisable() => _isDragging = false;
        public void OnBeginDrag(PointerEventData eventData)
        {
            cursorArrowLine.SetActive(true);
            cursorArrowHead.SetActive(true);
            cardObject.SetActive(false);

            if (!_isDragging)
            {
                _startPosition = _cardRectTransform.position;
                _isDragging = true;
            }
        }
    }
}