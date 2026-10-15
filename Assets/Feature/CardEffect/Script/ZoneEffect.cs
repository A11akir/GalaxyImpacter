using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Зона: применяет вложенный сериализованный эффект к цели и её соседям по доске.
    /// Зона читается из Values по своей позиции: 3 = цель + по 1 соседу,
    /// 5 = + по 2, 7 = + по 3. Вложенный эффект читает своё число из Values
    /// по позиции zone + 1 и должен иметь TargetSelection = Target.
    /// </summary>
    [System.Serializable]
    public class ZoneEffect : CardEffect
    {
        [SerializeReference] private CardEffect _effect;

        public override void Execute(EffectContext context)
        {
            if (_effect == null) return;

            var values = context.CardData.Values;
            if (values == null || context.ValueIndex >= values.Count) return;

            int zone = values[context.ValueIndex];
            int radius = Mathf.Max(0, (zone - 1) / 2);

            int savedIndex = context.ValueIndex;
            var savedTarget = context.Target;

            foreach (var center in ResolveTargets(context))
            {
                if (center == null) continue;

                foreach (var zoneTarget in ResolveZoneTargets(center, radius, context))
                {
                    context.Target = zoneTarget;
                    context.ValueIndex = savedIndex + 1;
                    _effect.Execute(context);
                }
            }

            context.Target = savedTarget;
            context.ValueIndex = savedIndex;
        }

        private List<CardAndHealthEntityOwnerData> ResolveZoneTargets(
            CardAndHealthEntityOwnerData center, int radius, EffectContext ctx)
        {
            var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(center);
            if (playerData == null)
                return new List<CardAndHealthEntityOwnerData> { center };

            var slots = ctx.BattlefieldSystem.BoardManager.GetEntitySlotsForTargets(playerData);

            int idx = slots.IndexOf(center);
            if (idx < 0)
                return new List<CardAndHealthEntityOwnerData> { center };

            var result = new List<CardAndHealthEntityOwnerData>();
            for (int p = idx - radius; p <= idx + radius; p++)
            {
                if (p >= 0 && p < slots.Count)
                    result.Add(slots[p]);
            }

            return result;
        }
    }
}
