// PassiveEffectsContainerView.cs
using System.Collections.Generic;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public class PassiveEffectsContainerView : MonoBehaviour
    {
        [SerializeField] private List<PassiveEffectIconView> _pool;

        private bool _isHovered;

        public PassiveEffectIconView GetFreeSlot()
        {
            foreach (var icon in _pool)
                if (!icon.IsInUse) return icon;

            Debug.LogWarning("No free passive effect icon slots in pool!");
            return null;
        }

        public void SetHovered(bool hovered)
        {
            _isHovered = hovered;
            foreach (var icon in _pool)
                icon.SetHoverState(hovered);
        }

        public void HideAll()
        {
            foreach (var icon in _pool)
                icon.ForceHide();
        }
    }
}