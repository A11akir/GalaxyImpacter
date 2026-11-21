using System;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public abstract class CritBuff : PassiveEffectBase, ICardContextConsumer, IStackablePassive, IValueProvider
    {
        [SerializeField] protected bool _doubleChance;

        private readonly ReactiveProperty<int> _bonus = new(0);

        public ReadOnlyReactiveProperty<int> Value => _bonus;
        
        public bool DoubleChance => _doubleChance;

        public virtual int DamageBonus => 0;
        public virtual int ChanceBonus => 0;

        public virtual void OnAppliedFromCard(EffectContext context)
        {
            // бафф может быть без своего велью (например, только флаг DoubleChance)
            var values = context.CardData?.Values;

            if (values == null || context.ValueIndex >= values.Count) return;

            AddBonus(values[context.ValueIndex]);
        }

        public void AddBonus(int amount) => _bonus.Value += amount;

        public void SetBonus(int amount) => _bonus.Value = amount;

        public void SetDoubleChance(bool value) => _doubleChance = value;

        public bool CanStackWith(EffectContext context) => true;

        public void PrepareStack(DurationType duration) { }

        public override void Register(CardAndHealthEntityOwnerData owner) { }
        public override void Unregister() { }

        protected void CopyFields(CritBuff clone)
        {
            clone._doubleChance = _doubleChance;
            CopyBaseFields(clone);
        }
    }
}
