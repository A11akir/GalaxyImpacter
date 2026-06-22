// PassiveEffectsPresenter.cs — следит за тем же списком, только для UI

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
        private readonly Dictionary<PassiveEffectBase, System.IDisposable> _valueSubscriptions = new();
        private List<PassiveEffectBase> _previousList = new();

        public PassiveEffectsPresenter(PassiveEffectsContainerView view, PassiveEffectsData data)
        {
            _view = view;
            data.ActivePassives.Subscribe(HandleChanged);
        }

        private void HandleChanged(List<PassiveEffectBase> currentList)
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

            _activeIcons[passive] = icon;

            icon.SetIcon(passive.Icon);


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
                icon.HideValue(); // <-- скрываем число
                icon.SetDescription(passive.GetDescription(0));
            }
        }

        private void HandlePassiveRemoved(PassiveEffectBase passive)
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