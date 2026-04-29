using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Feature.Items.Scripts
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<ItemInventoryView> _items;
        
        private RectTransform _rectTransform;
        
        private const float VisibleX = 1035f;
        private const float HiddenX = 1200f;
        private const float AnimDuration = 0.6f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetPositionX(HiddenX);
        }

        public void SetViews(List<ItemData> data)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (i < data.Count && data[i] != null)
                {
                    _items[i].SetView(data[i]);
                    _items[i].gameObject.SetActive(true);
                }
                else
                {
                    _items[i].gameObject.SetActive(false);
                }
            }
        }

        public void Show()
        {
            AnimatePositionX(VisibleX, Ease.OutBack);
        }

        public void Hide()
        {
            AnimatePositionX(HiddenX, Ease.InBack);
        }

        private void AnimatePositionX(float targetX, Ease ease)
        {
            if (!_rectTransform) return;

            DOTween.To(
                    () => _rectTransform.anchoredPosition.x,
                    x => _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y),
                    targetX,
                    AnimDuration)
                .SetEase(ease);
        }

        private void SetPositionX(float x)
        {
            _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y);
        }
    }
}