using System;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class SummonEffect : CardEffect
    {
        [SerializeField] private MinionCardData _summonedMinion;

        public override void Execute(EffectContext context)
        {
            if (_summonedMinion == null) return;

            var playerData = context.CasterSide ?? context.GameSessionModel.GetPlayerDataByOwner(context.Caster);
            if (playerData == null)
            {
                return;
            }

            context.BattlefieldSystem.SummonMinion(playerData, _summonedMinion, context.CasterSlot);
        }
    }
}
