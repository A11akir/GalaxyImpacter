using System;
using System.Collections.Generic;
using System.Linq;
using Feature.CardEffect.Script;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class PassiveEffectsPresenter
    {
        private readonly PassiveEffectsContainerView _view;
        private readonly Dictionary<PassiveEffectBase, PassiveEffectIconView> _activeIcons = new();
        private readonly Dictionary<PassiveEffectBase, IDisposable> _valueSubscriptions = new();
        private readonly IDisposable _subscription;

        private List<PassiveEffectBase> _previousList = new();

        public PassiveEffectsPresenter(PassiveEffectsContainerView view, PassiveEffectsData data)
        {
            _view = view;

            _subscription = data.ActivePassives.Subscribe(HandlePassivesChanged);
        }

        private void HandlePassivesChanged(List<PassiveEffectBase> currentList)
        {
            var added = currentList.Except(_previousList);
            var removed = _previousList.Except(currentList);

            foreach (var passive in removed)
                HandlePassiveRemoved(passive);

            foreach (var passive in added)
                HandlePassiveAdded(passive);

            _previousList = new List<PassiveEffectBase>(currentList);
        }

        private void HandlePassiveAdded(PassiveEffectBase passive)
        {
            if (passive.Icon == null) return;

            var icon = _view.GetFreeSlot();
            if (icon == null) return;

            icon.gameObject.SetActive(true);
            icon.SetIcon(passive.Icon);
            _activeIcons[passive] = icon;

            if (passive is IValueProvider valueProvider)
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

        private void HandlePassiveRemoved(PassiveEffectBase passive)
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
            _subscription.Dispose();
            foreach (var sub in _valueSubscriptions.Values)
                sub.Dispose();
            _valueSubscriptions.Clear();
        }
    }
}