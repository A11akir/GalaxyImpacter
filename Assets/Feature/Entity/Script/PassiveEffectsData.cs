using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using R3;

namespace Feature.Entity.Script
{
    [Serializable]
    public class PassiveEffectsData : IDisposable
    {
        private CardAndHealthEntityOwnerData _owner;
        
        private readonly ReactiveProperty<List<PassiveEffect.Script.PassiveEffect>> _activePassives = 
            new(new List<PassiveEffect.Script.PassiveEffect>());
        
        public ReadOnlyReactiveProperty<List<PassiveEffect.Script.PassiveEffect>> ActivePassives => _activePassives;
        public IReadOnlyList<PassiveEffect.Script.PassiveEffect> PassivesList => _activePassives.Value;

        public event Action<PassiveEffect.Script.PassiveEffect> OnPassiveAdded;
        public event Action<PassiveEffect.Script.PassiveEffect> OnPassiveRemoved;

        public void Initialize(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
        }

        public void AddPassive(
            PassiveEffect.Script.PassiveEffect passive, 
            CombatSystem.CombatSystem combatSystem)
        {
            if (passive == null || _activePassives.Value.Contains(passive)) return;
            if (_owner == null) 
                throw new InvalidOperationException("PassiveEffectsData not initialized with owner");

            var newList = new List<PassiveEffect.Script.PassiveEffect>(_activePassives.Value);
            newList.Add(passive);
            _activePassives.Value = newList;

            passive.Register(_owner, combatSystem);
            OnPassiveAdded?.Invoke(passive);
        }

        public void RemovePassive(PassiveEffect.Script.PassiveEffect passive)
        {
            if (passive == null || !_activePassives.Value.Contains(passive)) return;

            var newList = new List<PassiveEffect.Script.PassiveEffect>(_activePassives.Value);
            newList.Remove(passive);
            _activePassives.Value = newList;

            passive.Unregister();
            OnPassiveRemoved?.Invoke(passive);
        }

        public void RemoveAllPassives()
        {
            foreach (var passive in _activePassives.Value)
            {
                passive.Unregister();
            }
            
            _activePassives.Value = new List<PassiveEffect.Script.PassiveEffect>();
        }

        public T GetPassive<T>() where T : PassiveEffect.Script.PassiveEffect
        {
            foreach (var passive in _activePassives.Value)
            {
                if (passive is T typedPassive) return typedPassive;
            }
            return null;
        }

        public void Dispose()
        {
            RemoveAllPassives();
            OnPassiveAdded = null;
            OnPassiveRemoved = null;
            _owner = null;
        }
    }
}