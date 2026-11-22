using System;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Призывает существо на поле. Кого призвать — задаётся фильтром,
    /// как у GenerateTemporaryHandCardEffect: конкретная карта, класс, редкость и т.д.
    /// </summary>
    [Serializable]
    public class SummonEffect : CardEffect
    {
        [SerializeField] private CardPickFilter filter = new();

        public override void Execute(EffectContext context)
        {
            var playerData = context.CasterSide ?? context.GameSessionModel.GetPlayerDataByOwner(context.Caster);
            if (playerData == null) return;

            var minion = context.CardPoolPickSystem.Pick(filter, context) as MinionCardData;
            if (minion == null) return;

            context.BattlefieldSystem.SummonMinion(playerData, minion, context.CasterSlot);
        }
    }
}
