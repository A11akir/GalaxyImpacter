using System;
using Feature.GameSessionData;
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
        [Inject] private CastCardAreaMinion _castCardAreaMinion;
        private RectTransform _rectTransform;
        private int _hierarchyIndex;

        public bool _canCastCard { get; set; }

        private CardAndHealthEntityOwnerData _owner;
        public event Action<CardAndHealthEntityOwnerData> OnTryCardCast;

        [Inject] private HandCardsPositionSystem _handCardsPositionSystem;
        

        public void SetOwner(CardAndHealthEntityOwnerData owner) => _owner = owner;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();


        private void OnDisable()
        {
            ResetTransform();
            _handCardsPositionSystem?.UpdateCardsPosition(transform.parent);
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
            if (isDrag) return;
            ResetTransform();
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition(transform.parent);
        }

        #endregion

        #region Drag

        public void OnDrag(PointerEventData eventData)
        {
            _castCardAreaMinion.gameObject.SetActive(true);
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

            _castCardAreaMinion.CheckCardAreaSpell(transform);
            _castCardAreaMinion.CardGoingIsUsed = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            TryCastCard(this);
            _castCardAreaMinion.CardIsAreaAllTargetUseEffectOff();
            _castCardAreaMinion.CardGoingIsUsed = false;
            
            DragCancel();
        }

        private void DragCancel()
        {
            isDrag = false;
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition(transform.parent);
            _castCardAreaMinion.gameObject.SetActive(false);
        }

        #endregion

        public void TryCastCard(ITransformCastCardBehaviour currentCardBehaviour)
        {
            if (_castCardAreaMinion.CardHasTarget)
                OnTryCardCast?.Invoke(_owner);
        }

        public void CanCastCard(bool canCast) => _canCastCard = canCast;
    }
}

