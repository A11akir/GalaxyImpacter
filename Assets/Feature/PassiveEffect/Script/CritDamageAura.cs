using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Крит, который стакается: каждая карта добавляет процент урона и задаёт шанс.
    /// Стакается только с критом с таким же шансом, иначе создаётся новый эффект.
    /// Каждый стак помнит свою длительность — «до конца хода» снимается отдельно.
    /// </summary>
    [Serializable]
    public class CritDamageAura : DamageBonusAura
    {
        [SerializeReference] private List<ICritModifier> _modifiers = new();

        private class Stack
        {
            public int DamagePercent;
            public DurationType Duration;
        }

        private readonly List<Stack> _stacks = new();
        private CritState _state = new();
        private CardAndHealthEntityOwnerData _owner;
        private DurationType _incomingDuration;
        private bool _hasIncomingDuration;

        public override string GetDescription(int value)
        {
            if (Config == null) return base.GetDescription(value);
            return string.Format(Config.Description, Value.CurrentValue, _state.ChancePercent);
        }

        public override bool CanStackWith(EffectContext context) =>
            _state.ChancePercent == ReadIncoming(context).ChancePercent;

        public override void PrepareStack(DurationType duration)
        {
            _incomingDuration = duration;
            _hasIncomingDuration = true;

            // инстанс должен жить, пока жива самая долгая из его частей
            if (duration == DurationType.Permanent)
                Duration = DurationType.Permanent;
        }

        public override void OnAppliedFromCard(EffectContext context)
        {
            int saved = context.ValueIndex;

            var incoming = ReadIncoming(context);
            int damagePercent = context.CardData.Values[saved] + incoming.DamagePercent;

            if (_stacks.Count == 0)
                _state.ChancePercent = incoming.ChancePercent;

            _stacks.Add(new Stack
            {
                DamagePercent = damagePercent,
                Duration = _hasIncomingDuration ? _incomingDuration : Duration
            });

            AddBonus(damagePercent);

            context.ValueIndex = saved;
        }

        private CritState ReadIncoming(EffectContext context)
        {
            int saved = context.ValueIndex;
            var state = new CritState();

            for (int m = 0; m < _modifiers.Count; m++)
            {
                var modifier = _modifiers[m];
                if (modifier == null) continue;

                context.ValueIndex = saved + 1 + m;
                modifier.Apply(state, context);
            }

            context.ValueIndex = saved;
            return state;
        }

        public override int GetDamageBonus(CardStatsData sourceCard) => 0;

        public override float GetDamageMultiplier(CardStatsData sourceCard) =>
            Random.Range(0, 100) < _state.ChancePercent
                ? Value.CurrentValue / 100f
                : 1f;

        protected override void OnTurnTick()
        {
            int removed = 0;

            for (int i = _stacks.Count - 1; i >= 0; i--)
            {
                if (_stacks[i].Duration != DurationType.UntilTurnEnd) continue;

                removed += _stacks[i].DamagePercent;
                _stacks.RemoveAt(i);
            }

            if (removed == 0) return;

            AddBonus(-removed);

            if (_stacks.Count == 0)
                _owner?.PassiveEffects.Remove(this);
        }

        public override void Register(CardAndHealthEntityOwnerData owner) => _owner = owner;

        public override PassiveEffectBase Clone()
        {
            var clone = new CritDamageAura
            {
                _modifiers = _modifiers
            };
            CopyBaseFields(clone);
            return clone;
        }
    }
}
