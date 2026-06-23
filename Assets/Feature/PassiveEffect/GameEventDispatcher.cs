using Feature.GameSessionData;
using Feature.PassiveEffect.Script;

namespace Feature.PassiveEffect
{
    public class GameEventDispatcher
    {
        public void Notify<TEvent>(CardAndHealthEntityOwnerData owner, TEvent gameEvent)
        {
            foreach (var passive in owner.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is IGameEventListener<TEvent> listener)
                    listener.OnEvent(gameEvent);
        }
    }
}