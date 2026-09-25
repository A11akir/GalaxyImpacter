using System.Collections.Generic;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public class PassiveEffectsContainerView : MonoBehaviour
    {
        [SerializeField] private List<EffectIconViewBase> _pool;

        public EffectIconViewBase GetFreeSlot()
        {
            foreach (var icon in _pool)
                if (!icon.IsInUse) return icon;

            Debug.LogWarning("No free effect icon slots in pool!");
            return null;
        }

        public void SetHovered(bool hovered)
        {
            foreach (var icon in _pool)
            {
                if (icon is PassiveEffectIconView passiveIcon)
                    passiveIcon.SetHoverState(hovered);
                else if (icon is EffectHeroOnBoardView boardIcon)
                    boardIcon.SetBoardHoverState(hovered);
            }
        }

        public void HideAll()
        {
            foreach (var icon in _pool)
                icon.ForceHide();
        }
    }
}