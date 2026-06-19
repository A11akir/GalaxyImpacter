using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using R3;
using UnityEngine;

namespace Feature.Entity.Script
{
    public class PassiveEffectsPresenter : IDisposable
    {
        private readonly PassiveEffectsContainerView _view;
        private readonly PassiveEffectsData _data;
        private readonly Dictionary<PassiveEffect.Script.PassiveEffect, PassiveEffectIconView> _activeIcons = new();
        private readonly Dictionary<PassiveEffect.Script.PassiveEffect, IDisposable> _valueSubscriptions = new();

        public PassiveEffectsPresenter(PassiveEffectsContainerView view, PassiveEffectsData data)
        {
            _view = view;
            _data = data;

            _data.OnPassiveAdded += HandlePassiveAdded;
            _data.OnPassiveRemoved += HandlePassiveRemoved;

            foreach (var passive in data.PassivesList)
                HandlePassiveAdded(passive);
        }

        private void HandlePassiveAdded(PassiveEffect.Script.PassiveEffect passive)
        {
            Debug.Log($"[Presenter] HandlePassiveAdded: passive={passive.GetType().Name}, Icon={passive.Icon}");

            if (passive.Icon == null)
            {
                Debug.Log("[Presenter] SKIPPED, Icon is null");
                return;
            }
            var icon = _view.GetFreeIcon();
            if (icon == null) return;

            icon.gameObject.SetActive(true);
            icon.SetIcon(passive.Icon);
            _activeIcons[passive] = icon;

            if (passive is IStackablePassive stackable && passive is IValueProvider valueProvider)
            {
                var sub = valueProvider.Value.Subscribe(value =>
                {
                    icon.SetValue(value);
                    icon.SetDescription(passive.GetDescription(value));
                });
                _valueSubscriptions[passive] = sub;
            }
            else
            {
                icon.SetDescription(passive.GetDescription(0));
            }
        }

        private void HandlePassiveRemoved(PassiveEffect.Script.PassiveEffect passive)
        {
            if (_activeIcons.TryGetValue(passive, out var icon))
            {
                icon.gameObject.SetActive(false);
                _activeIcons.Remove(passive);
            }

            if (_valueSubscriptions.TryGetValue(passive, out var sub))
            {
                sub.Dispose();
                _valueSubscriptions.Remove(passive);
            }
        }

        public void Dispose()
        {
            _data.OnPassiveAdded -= HandlePassiveAdded;
            _data.OnPassiveRemoved -= HandlePassiveRemoved;

            foreach (var sub in _valueSubscriptions.Values)
                sub.Dispose();
            _valueSubscriptions.Clear();
        }
    }
}