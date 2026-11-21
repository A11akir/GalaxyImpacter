using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class ZoneRandomTargetModifier : ITargetModifier
    {
        private int _zone;

        public void ReadValue(EffectContext context) => _zone = context.Value();

        public List<CardAndHealthEntityOwnerData> ModifyTargets(
            List<CardAndHealthEntityOwnerData> targets, EffectContext context)
        {
            var center = PickRandomCenter(context);
            if (center == null) return targets;

            return ZoneTargetResolver.Expand(
                new List<CardAndHealthEntityOwnerData> { center }, _zone, context);
        }

        private static CardAndHealthEntityOwnerData PickRandomCenter(EffectContext context)
        {
            var effect = GetCurrentEffect(context);
            var pool = effect?.GetTargetsPool(context);

            if (pool == null || pool.Count <= 1)
                pool = TargetScopeResolver.Resolve(
                    TargetSelectionType.All, context.Caster, null, context.GameSessionModel);

            return pool.Count == 0 ? null : pool[UnityEngine.Random.Range(0, pool.Count)];
        }

        private static CardEffect GetCurrentEffect(EffectContext context)
        {
            var effects = context.CurrentEffectsList;
            if (effects == null) return null;
            if (context.ValueIndex < 0 || context.ValueIndex >= effects.Count) return null;

            return effects[context.ValueIndex];
        }
    }
}
