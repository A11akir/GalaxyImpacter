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
            var values = context.CardData?.Values;
            bool hasValue = values != null && context.ValueIndex >= 0 && context.ValueIndex < values.Count;

            if (hasValue)
            {
                AddBonus(values[context.ValueIndex]);
                return;
            }

            // без велью бафф имеет смысл только как флаг (например, двойной шанс крита)
            if (!_doubleChance)
                Debug.LogError($"[CritBuff] нет велью для слота {context.ValueIndex} в карте {context.CardData?.Name}");
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
