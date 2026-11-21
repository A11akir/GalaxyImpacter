using System;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Получаемый этим существом урон умножается.
    /// Велью: слот 0 — множитель (2 = в два раза больше).
    /// </summary>
    [Serializable]
    public class DamageMultiplierPassive : PassiveEffectBase, IDamageReaction, ICardContextConsumer
    {
        private int _multiplier;

        public void OnAppliedFromCard(EffectContext context)
        {
            _multiplier = context.Value();
        }

        public override void Register(CardAndHealthEntityOwnerData owner) => Debug.Log($"[DamageMultiplier] register on {owner._heroName}");

        public override void Unregister() { }

        // после уворота (0), до остальных реакций, чтобы те считали уже умноженный урон
        public int Priority => 5;

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, ref int finalDamage, CardAndHealthEntityOwnerData source)
        {
            finalDamage *= _multiplier;
            return false;   // урон не блокируется, только умножается
        }

        public override PassiveEffectBase Clone()
        {
            var clone = new DamageMultiplierPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}
