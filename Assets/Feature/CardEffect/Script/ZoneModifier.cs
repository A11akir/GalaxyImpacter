using System;
using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Модификатор зоны: своё велью = зона (3/5/7). Вкладывается в таргетный эффект —
    /// его цели разворачиваются в зону (цель + соседи).
    /// </summary>
    [Serializable]
    public class ZoneModifier : ITargetModifier
    {
        public List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context) =>
            ZoneTargetResolver.Expand(targets, context.Value(), context);
    }
}
