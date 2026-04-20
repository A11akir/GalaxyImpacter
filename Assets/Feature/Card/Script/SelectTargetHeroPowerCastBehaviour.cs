using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.Card.Script
{
    public class SelectTargetHeroPowerCastBehaviour : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        ITransformCastCardBehaviour
    {
        [Inject] private TargetingSystem _targetingSystem;
        [Inject] private GraphicRaycaster _raycaster;
        [Inject] private EventSystem _eventSystem;

        private RectTransform _rectTransform;
        private RectTransform _lineRectTransform;
        private RectTransform _headRectTransform;
        private GameObject _arrowHead;
        private GameObject _arrowLine;

        private Vector2 _startLocalPosition;
        private Vector2 _startPosition;
        private bool _isDragging;

        public bool _canCastCard { get; set; }
        private CardAndHealthEntityOwnerData _owner;
        private CardAndHealthEntityOwnerData _currentTarget;

        public event Action<CardAndHealthEntityOwnerData, CardAndHealthEntityOwnerData> OnTryCardCast;

        public void SetOwner(CardAndHealthEntityOwnerData owner) => _owner = owner;
        public void CanCastCard(bool canCast) => _canCastCard = canCast;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startLocalPosition = _rectTransform.localPosition;
        }

        public void Init(GameObject arrowHead, GameObject arrowLine)
        {
            _arrowHead = arrowHead;
            _arrowLine = arrowLine;
            _lineRectTransform = arrowLine.GetComponent<RectTransform>();
            _headRectTransform = arrowHead.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_canCastCard) return;
            _isDragging = true;
            _startPosition = eventData.pressEventCamera.WorldToScreenPoint(_rectTransform.position);
            _arrowHead.SetActive(true);
            _arrowLine.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canCastCard || !_isDragging) return;

            UpdateCursorArrow(eventData);
            UpdateCurrentTarget(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            _arrowHead.SetActive(false);
            _arrowLine.SetActive(false);
            TryCastCard(this);
            _currentTarget = null;
            _rectTransform.localPosition = _startLocalPosition;
        }

        public void TryCastCard(ITransformCastCardBehaviour currentCardBehaviour)
        {
            if (_currentTarget != null)
                OnTryCardCast?.Invoke(_owner, _currentTarget);
        }

        private void UpdateCursorArrow(PointerEventData eventData)
        {
            RectTransform arrowParentRect = _lineRectTransform.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                arrowParentRect, eventData.position, eventData.pressEventCamera,
                out Vector2 currentLocalPoint);
            
            Vector2 heroPowerScreenPos = eventData.pressEventCamera.WorldToScreenPoint(_rectTransform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                arrowParentRect, heroPowerScreenPos, eventData.pressEventCamera,
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
    }
}