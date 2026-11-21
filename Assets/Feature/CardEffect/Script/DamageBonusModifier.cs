using System;
using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Модификатор урона: своё велью прибавляется к урону атаки. Цели не меняет.
    /// </summary>
    [Serializable]
    public class DamageBonusModifier : ITargetModifier, IDamageBonusModifier
    {
        private int _bonus;

        public int DamageBonus => _bonus;

        public void ReadValue(EffectContext context) => _bonus = context.Value();

        public List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context) => targets;
    }
}
