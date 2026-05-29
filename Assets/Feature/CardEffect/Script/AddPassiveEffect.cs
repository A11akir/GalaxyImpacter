using System;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddPassiveEffect : CardEffect
    {
        [UnityEngine.SerializeReference]
        private PassiveEffect.Script.PassiveEffect _passive;

        public override void Execute(EffectContext ctx)
        {
            if (_passive == null) return;
            ctx.Caster.AddPassive(_passive, ctx.CombatSystem);
        }
    }
}
