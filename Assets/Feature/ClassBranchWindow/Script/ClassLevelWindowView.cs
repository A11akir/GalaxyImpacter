// ClassLevelWindowView.cs

using System.Collections.Generic;
using DG.Tweening;
using Feature.Hero;
using UnityEngine;

namespace Feature.ClassBranchWindow.Script
{
    public class ClassLevelWindowView : MonoBehaviour
    {
        [SerializeField] private List<ClassLevelEntryView> _entries;
        
        private RectTransform _rectTransform;
        
        private const float VisibleY = 250f;
        private const float HiddenY = -250f;
        private const float AnimDuration = 0.6f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            SetPositionY(HiddenY);
            
            foreach (var entry in _entries)
                entry.gameObject.SetActive(false);
        }

        public void UpdateEntry(AllHeroClass heroClass, int level, Color color)
        {
            var entry = _entries.Find(e => e.gameObject.activeSelf && e.HeroClass == heroClass);
    
            if (level > 0)
            {
                if (entry == null)
                    entry = _entries.Find(e => !e.gameObject.activeSelf);
        
                if (entry == null) return; 
        
                entry.gameObject.SetActive(true);
                entry.SetView(heroClass, level, color);
            }
            else if (entry != null)
            {
                entry.gameObject.SetActive(false);
            }
        }

        public void Show() => AnimatePositionY(VisibleY, Ease.OutBack);
        public void Hide() => AnimatePositionY(HiddenY, Ease.InBack);

        private void AnimatePositionY(float targetY, Ease ease)
        {
            if (!_rectTransform) return;

            DOTween.To(
                    () => _rectTransform.anchoredPosition.y,
                    y => _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, y),
                    targetY,
                    AnimDuration)
                .SetEase(ease);
        }

        private void SetPositionY(float y)
        {
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, y);
        }
    }
}