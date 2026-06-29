using Feature.CardEffect.Script;
using Feature.Health;

namespace Feature.Entity.Script
{
    public interface IEntityView : IHealthView
    {
        PassiveEffectsContainerView PassiveEffectsView { get; }
    }
}