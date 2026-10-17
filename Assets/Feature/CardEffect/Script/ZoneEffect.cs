using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [System.Serializable]
    public class ZoneEffect : CardEffect
    {
        //вложенный эффект
        [SerializeReference] private CardEffect _effect;

        public override void Execute(EffectContext context)
        {
            if (_effect == null) return;

            //велью зоны
            int zone = context.Value();

            //число насколько бить в сторону
            int radius = Mathf.Max(0, (zone - 1) / 2);

            foreach (var center in ResolveTargets(context))
            {
                foreach (var zoneTarget in ResolveZoneTargets(center, radius, context))
                {
                    ExecuteInSlot(context, 1, ctx =>
                    {
                        ctx.Target = zoneTarget;
                        _effect.Execute(ctx);
                    });
                }
            }
        }

        private List<CardAndHealthEntityOwnerData> ResolveZoneTargets(
            CardAndHealthEntityOwnerData center, int radius, EffectContext ctx)
        {
            var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(center);

            var slots = ctx.BattlefieldSystem.BoardManager.GetEntitySlotsForTargets(playerData);

            int idx = slots.IndexOf(center);

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
