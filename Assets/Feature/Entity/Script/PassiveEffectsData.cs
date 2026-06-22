using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.PassiveEffect.Script;
using R3;
using UnityEngine;

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
            // Снимок текущего состояния на начало фазы
            var startPassives = new List<PassiveEffectBase>(_activePassives.Value);


            // 1. Сначала выполняем эффекты конца хода
            foreach (var passive in startPassives)
            {
                if (passive is TurnEndEffectPassive)
                {
                    passive.TickTurnEnd();
                }
            }


            // 2. После выполнения эффектов берём актуальный список
            // потому что TurnEndEffectPassive мог создать новые пассивки
            var allPassives = new List<PassiveEffectBase>(_activePassives.Value);


            // 3. Собираем всё что должно исчезнуть
            var toRemove = new List<PassiveEffectBase>();

            foreach (var passive in allPassives)
            {
                if (passive.Duration == DurationType.UntilTurnEnd)
                {
                    toRemove.Add(passive);
                }
            }


            // 4. Удаляем после завершения всех эффектов
            foreach (var passive in toRemove)
            {
                passive.Unregister();
                Remove(passive);
            }
        }
    }
}