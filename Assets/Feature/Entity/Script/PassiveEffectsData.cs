using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class PassiveEffectsData
    {
        private readonly CardAndHealthEntityOwnerData _owner;

        private readonly ReactiveProperty<List<PassiveEffectBase>> _activePassives =
            new(new List<PassiveEffectBase>());

        public ReadOnlyReactiveProperty<List<PassiveEffectBase>> ActivePassives => _activePassives;

        public PassiveEffectsData(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
        }

        public void AddPassive(PassiveEffectBase passive, CombatSystem.CombatSystem combatSystem)
        {
            if (passive == null)
                throw new ArgumentNullException(nameof(passive));

            if (_activePassives.Value.Contains(passive))
                throw new InvalidOperationException(
                    $"Passive '{passive.GetType().Name}' is already added to this owner.");

            passive.Register(_owner, combatSystem);

            var newList = new List<PassiveEffectBase>(_activePassives.Value);
            newList.Add(passive);
            _activePassives.Value = newList;
        }

        public void RemovePassive(PassiveEffectBase passive)
        {
            if (passive == null || !_activePassives.Value.Contains(passive)) return;

            passive.Unregister();

            var newList = new List<PassiveEffectBase>(_activePassives.Value);
            newList.Remove(passive);
            _activePassives.Value = newList;
        }

        public void OnTurnEnd()
        {
            var toRemove = new List<PassiveEffectBase>();

            foreach (var passive in _activePassives.Value)
                if (passive.TickTurnEnd())
                    toRemove.Add(passive);

            foreach (var p in toRemove)
                RemovePassive(p);
        }

        public PassiveEffectBase Find(Type type)
        {
            foreach (var passive in _activePassives.Value)
                if (passive.GetType() == type)
                    return passive;
            return null;
        }

        public T Find<T>() where T : PassiveEffectBase
        {
            foreach (var passive in _activePassives.Value)
                if (passive is T typed)
                    return typed;
            return null;
        }

        public PassiveEffectBase Create(PassiveEffectBase template, CombatSystem.CombatSystem combatSystem)
        {
            var newPassive = template.Clone();
            AddPassive(newPassive, combatSystem);
            return newPassive;
        }
    }
}