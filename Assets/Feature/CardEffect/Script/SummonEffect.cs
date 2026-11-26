using System;
using System.Linq;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Призывает существо на поле. Кого призвать — задаётся фильтром,
    /// как у GenerateTemporaryHandCardEffect: конкретная карта, класс, редкость и т.д.
    /// Галочка Target Summoned — следующий эффект получает целью заспавненное существо
    /// (у него должен быть TargetSelection = Target).
    /// </summary>
    [Serializable]
    public class SummonEffect : CardEffect
    {
        [SerializeField] private CardPickFilter filter = new();
        [SerializeField] private bool _targetSummoned;

        public override int ControlledFollowingCount => _targetSummoned ? 1 : 0;

        public override void Execute(EffectContext context)
        {
            var playerData = context.CasterSide ?? context.GameSessionModel.GetPlayerDataByOwner(context.Caster);
            if (playerData == null) return;

            var minion = context.CardPoolPickSystem.Pick(filter, context) as MinionCardData;
            if (minion == null) return;

            var spawned = context.BattlefieldSystem.SummonMinion(playerData, minion, context.CasterSlot);

            if (!_targetSummoned || spawned == null) return;

            var savedTarget = context.Target;
            context.Target = spawned;

            ExecuteFollowingEffects(context, ControlledFollowingCount);

            context.Target = savedTarget;
        }
    }
}
