// ICardContextConsumer.cs

using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    public interface ICardContextConsumer
    {
        void OnAppliedFromCard(EffectContext context);
    }
}