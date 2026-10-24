using Feature.CardEffect.Script;
using Feature.Health;
using Feature.Shield;
using Feature.Shield.Script;

namespace Feature.Entity.Script
{
    public interface IEntityView : IHealthView
    {
        PassiveEffectsContainerView PassiveEffectsView { get; }
        ShieldView ShieldView { get; }
        void ShowDamage(int amount);
        
    }
}