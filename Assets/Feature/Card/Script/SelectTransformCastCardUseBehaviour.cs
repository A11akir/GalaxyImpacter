using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.HandLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.Card.Script
{
    public class SelectTransformCastCardUseBehaviour : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        ITransformCastCardBehaviour
    {
        [Header("Arrow")]
        public GameObject cursorArrowHead;
        public GameObject cursorArrowLine;
        public GameObject cardObject;

        [Header("Hover")]
         private float scaleFactor = 1.5f;

        [Inject] private HandCardsPositionSystem _handCardsPositionSystem;
        [Inject] private TargetingSystem _targetingSystem;
        
        private RectTransform _rectTransform;
        private RectTransform _lineRectTransform;
        private RectTransform _headRectTransform;
        private RectTransform _cardRectTransform;

        private Vector2 _startPosition;

        private bool _isDragging;
        private int _hierarchyIndex;
        private static bool _isPointerEnter;

        public bool _canCastCard { get; set; }
        
        private CardAndHealthEntityOwnerData _owner;
        private CardAndHealthEntityOwnerData _currentTarget;

        public event Action<CardAndHealthEntityOwnerData, CardAndHealthEntityOwnerData> OnTryCardCast;

        public void SetOwner(CardAndHealthEntityOwnerData owner) => _owner = owner;

        public void TryCastCard(ITransformCastCardBehaviour currentCardBehaviour)
        {
            if (_currentTarget != null)
                OnTryCardCast?.Invoke(_owner, _currentTarget);
        }

        #region Init

        public void Init(GameObject viewCardContainer, GameObject viewCursorArrowHead, GameObject viewCursorArrowLine)
        {
            cardObject = viewCardContainer;
            cursorArrowHead = viewCursorArrowHead;
            cursorArrowLine = viewCursorArrowLine;
            
            _rectTransform = GetComponent<RectTransform>();
            _lineRectTransform = cursorArrowLine.GetComponent<RectTransform>();
            _headRectTransform = cursorArrowHead.GetComponent<RectTransform>();
            _cardRectTransform = cardObject.GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            ResetTransform();
            _isDragging = false;
            _currentTarget = null;
            _handCardsPositionSystem?.UpdateCardsPosition(transform.parent);
        }

        private void ResetTransform()
        {
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        #endregion

        #region Hover

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isActiveAndEnabled || _isPointerEnter) return;

            _isPointerEnter = true;

            _hierarchyIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();

            transform.localPosition = new Vector3(
                transform.localPosition.x,
                ((_rectTransform.rect.height / 2) * scaleFactor) - 5,
                transform.localPosition.z);

            transform.localScale = Vector3.one * scaleFactor;
            transform.localRotation = Quaternion.identity;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isDragging) return;

            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition(transform.parent);
            _isPointerEnter = false;
        }

        #endregion

        #region Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_canCastCard) return;
            _isDragging = true;
    
            _startPosition = eventData.pressEventCamera.WorldToScreenPoint(_rectTransform.position);
    
            cardObject.SetActive(false); // ← скрываем карту
            cursorArrowHead.SetActive(true);
            cursorArrowLine.SetActive(true);
            transform.localScale = Vector3.one;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canCastCard || !_isDragging) return;

            UpdateCursorArrow(eventData);
            UpdateCurrentTarget(eventData);
        }

        [Inject] private GraphicRaycaster _raycaster;
        [Inject] private EventSystem _eventSystem;

        private void UpdateCurrentTarget(PointerEventData eventData)
        {
            _currentTarget = null;

            var results = new List<RaycastResult>();
            var pointerData = new PointerEventData(_eventSystem) { position = eventData.position };
            _raycaster.Raycast(pointerData, results);

            foreach (var result in results)
            {
                var target = GetTargetFromHierarchy(result.gameObject);
                if (target != null)
                {
                    _currentTarget = target;
                    break;
                }
            }
        }

        private CardAndHealthEntityOwnerData GetTargetFromHierarchy(GameObject go)
        {
            var current = go.transform;
            while (current != null)
            {
                var target = _targetingSystem.GetTarget(current.gameObject);
                if (target != null) return target;
                current = current.parent;
            }
            return null;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cursorArrowLine.SetActive(false);
            cursorArrowHead.SetActive(false);
            cardObject.SetActive(true);

            _isDragging = false;

            TryCastCard(this);
            _currentTarget = null;
            _isPointerEnter = false;

            DragCancel();
        }

        private void DragCancel()
        {

            
                transform.SetSiblingIndex(_hierarchyIndex);
                _handCardsPositionSystem.UpdateCardsPosition(transform.parent);
            
        }

        #endregion

        #region Arrow Logic

        private void UpdateCursorArrow(PointerEventData eventData)
        {
            RectTransform arrowParentRect = _lineRectTransform.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                arrowParentRect, eventData.position, eventData.pressEventCamera,
                out Vector2 currentLocalPoint);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                arrowParentRect, _startPosition, eventData.pressEventCamera,
                out Vector2 startLocalPoint);

            Vector2 direction = currentLocalPoint - startLocalPoint;
            float distance = direction.magnitude;

            if (distance <= 0.01f) return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _lineRectTransform.localPosition = startLocalPoint + direction * 0.5f;
            _lineRectTransform.rotation = Quaternion.Euler(0, 0, angle);
            _lineRectTransform.sizeDelta = new Vector2(distance, _lineRectTransform.sizeDelta.y);

            _headRectTransform.localPosition = currentLocalPoint;
            _headRectTransform.rotation = Quaternion.Euler(0, 0, angle + 180);
        }

        #endregion

        public void CanCastCard(bool canCast)
        {
            _canCastCard = canCast;
        }
    }
}
