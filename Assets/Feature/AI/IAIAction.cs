using Feature.Card.Script;
using Feature.GameSessionData;

namespace Feature.AI
{
    public interface IAIAction
    {
        int Cost { get; }
        TargetType TargetType { get; }
        bool DealsDamage(); // ← метод вместо свойства
        void Execute(CardAndHealthEntityOwnerData target);
    }
}