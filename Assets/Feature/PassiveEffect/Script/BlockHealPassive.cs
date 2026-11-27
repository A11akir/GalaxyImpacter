using System;
using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    /// <summary>
    /// Запрещает восстанавливать HP владельцу (максимальное HP не меняется).
    /// </summary>
    [Serializable]
    public class BlockHealPassive : PassiveEffectBase, IHealReaction
    {
        public override void Register(CardAndHealthEntityOwnerData owner) { }

        public override void Unregister() { }

        public int Priority => 0;

        public bool CanHeal(CardAndHealthEntityOwnerData target, int heal) => false;

        public override PassiveEffectBase Clone()
        {
            var clone = new BlockHealPassive();
            CopyBaseFields(clone);
            return clone;
        }
    }
}
