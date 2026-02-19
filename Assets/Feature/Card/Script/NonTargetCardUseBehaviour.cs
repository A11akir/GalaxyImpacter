using System;
using Feature.HandLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Feature.Card.Script
{
    public class NonTargetCardUseBehaviour : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IDragHandler,
        IEndDragHandler,
        ITargetCardBehaviour
    {
        [SerializeField] private float scaleFactor = 1.5f;

        [Inject] private HandCardsPositionSystem _handCardsPositionSystem;
        [Inject] private CastCardAreaAllTarget _castCardAreaAllTarget;

        private RectTransform _rectTransform;
        private int _hierarchyIndex;

        private bool _isDrag;
        private static bool _isPointerEnter;

        public bool _canCastCard { get; set; }

        public event Action OnTryCardCast;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            ResetTransform();
            _handCardsPositionSystem?.UpdateCardsPosition();
        }

        private void ResetTransform()
        {
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

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

            transform.localScale = _isDrag ? Vector3.one : Vector3.one * scaleFactor;
            transform.localRotation = Quaternion.identity;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition();
            _isPointerEnter = false;
        }

        #endregion

        #region Drag

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canCastCard) return;

            if (!_isDrag)
            {
                _isDrag = true;
                transform.localScale = Vector3.one;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            _rectTransform.localPosition = localPoint;

            _castCardAreaAllTarget.CheckCardArea();
            _castCardAreaAllTarget.CardGoingIsUsed = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _castCardAreaAllTarget.CardIsAreaAllTargetUseEffectOff();
            _castCardAreaAllTarget.CardGoingIsUsed = false;

            TryCastCard(this);

            DragCancel();
        }

        private void DragCancel()
        {
            _isDrag = false;
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition();
        }

        #endregion

        public void TryCastCard(ITargetCardBehaviour currentCardBehaviour)
        {
            if (_castCardAreaAllTarget.CardHasTarget)
            {
                OnTryCardCast?.Invoke();
            }
        }

        public void CanCastCard(bool canCast)
        {
            _canCastCard = canCast;
        }
    }
}
