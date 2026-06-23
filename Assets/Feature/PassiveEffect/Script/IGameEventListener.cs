
namespace Feature.PassiveEffect.Script
{
    public interface IGameEventListener<TEvent>
    {
        void OnEvent(TEvent gameEvent);
    }
}