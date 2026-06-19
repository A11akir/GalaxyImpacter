// PassiveEffectsContainerView.cs — пул иконок, без логики
using System.Collections.Generic;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    public class PassiveEffectsContainerView : MonoBehaviour
    {
        [SerializeField] private List<PassiveEffectIconView> _pool;

        public PassiveEffectIconView GetFreeIcon()
        {
            foreach (var icon in _pool)
                if (!icon.gameObject.activeSelf) return icon;

            Debug.LogWarning("No free passive effect icon slots in pool!");
            return null;
        }

        public void HideAll()
        {
            foreach (var icon in _pool)
                icon.gameObject.SetActive(false);
        }
    }
}