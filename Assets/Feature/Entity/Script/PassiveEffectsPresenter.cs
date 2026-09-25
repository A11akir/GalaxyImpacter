using System.Collections.Generic;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.PassiveEffect.Script;
using R3;
using UnityEngine;

namespace Feature.Entity.Script
{
    public class PassiveEffectsPresenter
    {
        private readonly PassiveEffectsContainerView _view;
        private readonly Dictionary<PassiveEffectBase, EffectIconViewBase> _activeIcons = new();
        private readonly Dictionary<PassiveEffectBase, System.IDisposable> _valueSubscriptions = new();

        public PassiveEffectsPresenter(PassiveEffectsContainerView view)
        {
            _view = view;
        }

        public void HandlePassiveAdded(PassiveEffectBase passive)
        {
            var icon = _view.GetFreeSlot();
            Debug.Log($"[PassiveEffectsPresenter] {passive.GetType().Name}, icon={(icon != null ? icon.name : "NULL — no free slot")}");
            if (icon == null) return;

            _activeIcons[passive] = icon;
            icon.SetIcon(passive.Icon);
            Debug.Log($"[PassiveEffectsPresenter] icon.SetIcon called, sprite={(passive.Icon != null ? "OK" : "NULL sprite")}");

            if (passive is IValueProvider valueProvider)
            {
                var sub = valueProvider.Value.Subscribe(value => UpdateIcon(icon, passive, value));
                _valueSubscriptions[passive] = sub;
            }
            else
            {
                UpdateIcon(icon, passive, null);
            }
        }

        private void UpdateIcon(EffectIconViewBase icon, PassiveEffectBase passive, int? value)
        {
            icon.SetValue(value);
            icon.SetDescription(passive.GetDescription(value ?? 0));
            icon.SetName(passive.SourceCard != null ? passive.SourceCard.Name : passive.GetType().Name);
        }

        public void HandlePassiveRemoved(PassiveEffectBase passive)
        {
            if (_activeIcons.TryGetValue(passive, out var icon))
            {
                icon.ForceHide();
                _activeIcons.Remove(passive);
            }

            if (_valueSubscriptions.TryGetValue(passive, out var sub))
            {
                sub.Dispose();
                _valueSubscriptions.Remove(passive);
            }
        }
    }
}