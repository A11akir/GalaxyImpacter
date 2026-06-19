using System;
using System.Collections.Generic;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class PassiveEffectsData
    {
        private readonly ReactiveProperty<List<PassiveEffectBase>> _activePassives =
            new(new List<PassiveEffectBase>());

        public ReadOnlyReactiveProperty<List<PassiveEffectBase>> ActivePassives => _activePassives;

        public void Add(PassiveEffectBase passive)
        {
            var newList = new List<PassiveEffectBase>(_activePassives.Value) { passive };
            _activePassives.Value = newList;
        }

        public void Remove(PassiveEffectBase passive)
        {
            var newList = new List<PassiveEffectBase>(_activePassives.Value);
            newList.Remove(passive);
            _activePassives.Value = newList;
        }
        
        public PassiveEffectBase Find(Type type)
        {
            foreach (var passive in _activePassives.Value)
                if (passive.GetType() == type)
                    return passive;
            return null;
        }

        public PassiveEffectBase Create(PassiveEffectBase template)
        {
            var newPassive = template.Clone();
            Add(newPassive);
            return newPassive;
        }
        
        public T Find<T>() where T : PassiveEffectBase
        {
            foreach (var passive in _activePassives.Value)
                if (passive is T typed)
                    return typed;
            return null;
        }
        
        public void TickTurnEnd()
        {
            var toRemove = new List<PassiveEffectBase>();

            foreach (var passive in _activePassives.Value)
                if (passive.TickTurnEnd())
                    toRemove.Add(passive);

            foreach (var p in toRemove)
                Remove(p);
        }
    }
}