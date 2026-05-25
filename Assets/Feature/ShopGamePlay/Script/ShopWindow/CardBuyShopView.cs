using DG.Tweening;
using Feature.Card.Script;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.ShopGamePlay.Script.ShopWindow
{
    public class CardBuyShopView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private HandCardView _handCardView;

        [SerializeField] private GameObject _descriptionWindow;
        [SerializeField] private Transform _containerForDescription;
        private Transform _nativeContainerForDescription;

        private CardStatsData _cardData;
        public System.Action<CardStatsData> OnCardClicked;

        private void Awake()
        {
            if (_descriptionWindow != null)
            {
                _nativeContainerForDescription = _descriptionWindow.transform.parent;
                _descriptionWindow.SetActive(false);
            }
        }

        public void SetView(CardStatsData cardData)
        {
            _cardData = cardData;
            _handCardView.SetDataView(cardData);
        
            if (_descriptionWindow && _nativeContainerForDescription)
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
            OnCardClicked?.Invoke(_cardData);
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
            _handCardView.transform.DOScale(Vector3.one * 1.2f, 0.15f)
                .OnComplete(() => _handCardView.transform.DOScale(Vector3.one, 0.1f));
        }

        public void PlayCannotAffordAnimation()
        {
            _handCardView.transform.DOShakePosition(0.3f, strength: 10f, vibrato: 20);
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
                rectTransform.anchoredPosition = new Vector2(225f, rectTransform.anchoredPosition.y);
            else if (outOfBoundsRight)
                rectTransform.anchoredPosition = new Vector2(-225f, rectTransform.anchoredPosition.y);
        }
    }
}