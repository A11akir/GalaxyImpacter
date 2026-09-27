using Feature.CardEffect.Script;
using Feature.GameSessionData;
using R3;

namespace Feature.PassiveEffect.Script
{
    public class DamageBonusAura : PassiveEffectBase, ICardContextConsumer, ITeamDamageModifier, IValueProvider, IStackablePassive
    {
        private readonly ReactiveProperty<int> _bonus = new(0);
        public ReadOnlyReactiveProperty<int> Value => _bonus;

        public void OnAppliedFromCard(EffectContext context) =>
            AddBonus(context.CardData.Values[context.ValueIndex]);

        public void AddBonus(int amount) =>
            _bonus.Value += amount;

        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }

        public int GetDamageBonus(CardStatsData sourceCard) => _bonus.Value;

        public override PassiveEffectBase Clone() =>
            new DamageBonusAura { Config = Config, Duration = Duration };
    }
}