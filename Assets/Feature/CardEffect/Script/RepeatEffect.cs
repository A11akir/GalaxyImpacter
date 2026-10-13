using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class RepeatEffect : CardEffect
    {
        // Сколько следующих эффектов карты повторяется.
        [SerializeField] private int _affectedCount = 1;

        // Не выбирать одну и ту же цель дважды в рамках серии повторов.
        [SerializeField] private bool _avoidDuplicateTargets;

        public bool AvoidDuplicateTargets => _avoidDuplicateTargets;

        public override int ControlledFollowingCount => Mathf.Max(0, _affectedCount);

        public override void Execute(EffectContext context)
        {
            int repeatCount = context.CardData.Values[context.ValueIndex];
            if (repeatCount <= 0) return;

            var savedUsedTargets = context.UsedTargets;
            if (_avoidDuplicateTargets && savedUsedTargets == null)
                context.UsedTargets = new HashSet<CardAndHealthEntityOwnerData>();

            for (int i = 0; i < repeatCount; i++)
                ExecuteFollowingEffects(context, _affectedCount);

            context.UsedTargets = savedUsedTargets;
        }
    }
}
