using System;
using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Модификатор зоны: своё велью = зона (3/5/7). Цели разворачиваются в зону
    /// (цель + соседи по слотам доски).
    /// </summary>
    [Serializable]
    public class ZoneModifier : ITargetModifier
    {
        private int _zone;

        public void ReadValue(EffectContext context) => _zone = context.Value();

        public List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context) =>
            ZoneTargetResolver.Expand(targets, _zone, context);
    }
}
