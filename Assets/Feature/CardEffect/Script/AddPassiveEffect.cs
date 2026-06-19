
using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddPassiveEffect : CardEffect
    {
        [SerializeReference]
        private PassiveEffect.Script.PassiveEffect _passiveTemplate;

        public override void Execute(EffectContext ctx)
        {
            Debug.Log($"[AddPassiveEffect] Execute called, _passiveTemplate={_passiveTemplate?.GetType().Name ?? "NULL"}, Caster={ctx.Caster}, CardData={ctx.CardData?.Name}");

            if (_passiveTemplate == null)
            {
                Debug.LogWarning("[AddPassiveEffect] _passiveTemplate is NULL, aborting");
                return;
            }

            int amount = ctx.CardData.Values.Count > ctx.ValueIndex
                ? ctx.CardData.Values[ctx.ValueIndex]
                : 0;
            Debug.Log($"[AddPassiveEffect] amount={amount}, ValueIndex={ctx.ValueIndex}, Values.Count={ctx.CardData.Values.Count}");

            var existing = ctx.Caster.PassiveEffects.GetPassive(_passiveTemplate.GetType());
            Debug.Log($"[AddPassiveEffect] existing={existing?.GetType().Name ?? "NULL"}");

            if (existing is IStackablePassive stackable)
            {
                Debug.Log("[AddPassiveEffect] Adding bonus to EXISTING passive");
                stackable.AddBonus(amount);
                return;
            }

            Debug.Log("[AddPassiveEffect] Cloning new passive instance");
            var newPassive = _passiveTemplate.Clone();
            Debug.Log($"[AddPassiveEffect] newPassive={newPassive?.GetType().Name ?? "NULL"}, calling AddPassive...");
            ctx.Caster.PassiveEffects.AddPassive(newPassive, ctx.CombatSystem);

            if (newPassive is IStackablePassive newStackable)
            {
                Debug.Log("[AddPassiveEffect] Adding bonus to NEW passive");
                newStackable.AddBonus(amount);
            }
        }
    }
}