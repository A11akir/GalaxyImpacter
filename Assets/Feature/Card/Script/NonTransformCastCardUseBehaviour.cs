using System;
using Feature.HandLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Feature.Card.Script
{
    public class NonTransformCastCardUseBehaviour : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IDragHandler,
        IEndDragHandler,
        ITransformCastCardBehaviour
    {
        [SerializeField] private float scaleFactor = 1.5f;
        
        private bool isDrag;
        [Inject] private HandCardsPositionSystem _handCardsPositionSystem;
        [Inject] private CastCardAreaAllTarget _castCardAreaAllTarget;
        
        
        private RectTransform _rectTransform;
        private int _hierarchyIndex;
        
        public bool _canCastCard { get; set; }

        public event Action OnTryCardCast;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

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
            _hierarchyIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
    
            transform.localPosition = new Vector3(transform.localPosition.x,
                ((_rectTransform.rect.height/2)*scaleFactor)-5, 
                transform.localPosition.z);
    
            if (isDrag) transform.localScale = Vector3.one;
            else transform.localScale *= scaleFactor;
    
            transform.localRotation = Quaternion.identity;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition();
        }

        #endregion

        #region Drag

        public void OnDrag(PointerEventData eventData)
        {
            _castCardAreaAllTarget.gameObject.SetActive(true);
            if (!_canCastCard) return;
            
            if (!isDrag)
            {
                isDrag = true;
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
            TryCastCard(this);
            _castCardAreaAllTarget.CardIsAreaAllTargetUseEffectOff();
            _castCardAreaAllTarget.CardGoingIsUsed = false;
            
            DragCancel();
        }

        private void DragCancel()
        {
            isDrag = false;
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition();
            _castCardAreaAllTarget.gameObject.SetActive(false);
        }

        #endregion

        public void TryCastCard(ITransformCastCardBehaviour currentCardBehaviour)
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
