using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;

namespace Feature.PassiveEffect
{
    public class GameEventDispatcher
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new();

        public void Subscribe<TEvent>(Action<TEvent> callback)
        {
            var type = typeof(TEvent);
            if (!_listeners.ContainsKey(type))
                _listeners[type] = new List<Delegate>();
            _listeners[type].Add(callback);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> callback)
        {
            if (_listeners.TryGetValue(typeof(TEvent), out var list))
                list.Remove(callback);
        }

        public void Notify<TEvent>(CardAndHealthEntityOwnerData owner, TEvent gameEvent)
        {
            foreach (var passive in owner.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IGameEventListener<TEvent> listener)
                    listener.OnEvent(gameEvent);

            if (_listeners.TryGetValue(typeof(TEvent), out var list))
                foreach (var callback in list)
                    ((Action<TEvent>)callback).Invoke(gameEvent);
        }
    }
}