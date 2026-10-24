using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Разворачивает список целей в зону: каждая цель + её соседи по слотам доски.
    /// Используется базовым ResolveTargets, когда следующему эффекту задана зона.
    /// </summary>
    public static class ZoneTargetResolver
    {
        public static List<CardAndHealthEntityOwnerData> Expand(
            List<CardAndHealthEntityOwnerData> centers, int zone, EffectContext context)
        {
            int radius = GetZoneRadius(zone);

            var zoneTargets = new List<CardAndHealthEntityOwnerData>();
            foreach (var center in centers)
            {
                if (center == null) continue;
                zoneTargets.AddRange(ResolveCenterZone(center, radius, context));
            }

            return zoneTargets.Distinct().ToList();
        }

        private static int GetZoneRadius(int zone) =>
            Mathf.Max(0, (zone - 1) / 2);

        private static List<CardAndHealthEntityOwnerData> ResolveCenterZone(
            CardAndHealthEntityOwnerData center, int radius, EffectContext context)
        {
            var playerData = context.GameSessionModel.GetPlayerDataByOwner(center);
            var slots = context.BattlefieldSystem.BoardManager.GetEntitySlotsForTargets(playerData);

            int centerIndex = slots.IndexOf(center);

            var result = new List<CardAndHealthEntityOwnerData>();
            for (int slot = centerIndex - radius; slot <= centerIndex + radius; slot++)
            {
                bool slotOnBoard = slot >= 0 && slot < slots.Count;
                if (slotOnBoard)
                    result.Add(slots[slot]);
            }

            return result;
        }
    }
}
