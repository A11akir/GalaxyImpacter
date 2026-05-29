using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddPassiveEffect : CardEffect
    {
        [SerializeReference]
        private PassiveEffect.Script.PassiveEffect _passive;

        public override void Execute(EffectContext ctx)
        {
            if (_passive == null) return;
            
            Debug.Log($"Adding passive {_passive.GetType().Name} to {ctx.Caster}");
            ctx.Caster.AddPassive(_passive, ctx.CombatSystem);
        }
    }
}
