using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class PassiveEffectsPresenter
    {
        private readonly PassiveEffectsContainerView _view;
        private readonly Dictionary<PassiveEffectBase, PassiveEffectIconView> _activeIcons = new();
        private readonly Dictionary<PassiveEffectBase, System.IDisposable> _valueSubscriptions = new();

        public PassiveEffectsPresenter(PassiveEffectsContainerView view)
        {
            _view = view;
        }

        public void HandlePassiveAdded(PassiveEffectBase passive)
        {
            var icon = _view.GetFreeSlot();
            if (icon == null) return;

            _activeIcons[passive] = icon;
            icon.SetIcon(passive.Icon);

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

        private void UpdateIcon(PassiveEffectIconView icon, PassiveEffectBase passive, int? value)
        {
            icon.SetValue(value);
            icon.SetDescription(passive.GetDescription(value ?? 0));
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