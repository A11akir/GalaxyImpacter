using Feature.HandLogic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Feature.Card.Script
{
    public class TransformCardHandLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float scaleFactor = 1.5f;

        private bool _isDrag;
        private static bool _isPointerEnter;
        
        [Inject] private HandCardsPositionSystem  _handCardsPositionSystem;
        
        private RectTransform _rectTransform;
        private int _hierarchyIndex;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isActiveAndEnabled) return;
            CardPointerEnter();
        }

        private void OnEnable() => Init();

        private void OnDisable()
        {
            ResetCardTransform();
            _handCardsPositionSystem?.UpdateCardsPosition(transform.parent);
        }
        
        private void ResetCardTransform()
        {
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        [Button]
        private void Init() => _rectTransform = GetComponent<RectTransform>();

        [Button]
        private void CardPointerEnter()
        {
            if (_isPointerEnter) return;
            _isPointerEnter = true;
            _hierarchyIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
    
            transform.localPosition = new Vector3(transform.localPosition.x,
                ((_rectTransform.rect.height/2)*scaleFactor)-5, 
                transform.localPosition.z);
    
            if (_isDrag) transform.localScale = Vector3.one;
            else transform.localScale *= scaleFactor;
    
            transform.localRotation = Quaternion.identity;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition(transform.parent);
            
            _isPointerEnter = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
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
        }
        
        public void OnEndDrag(PointerEventData eventData) => DragCancel();

        private void DragCancel()
        {
            _isDrag = false;
            transform.SetSiblingIndex(_hierarchyIndex);
            _handCardsPositionSystem.UpdateCardsPosition(transform.parent);
        }
    }
}