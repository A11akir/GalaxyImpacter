using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class RepeatEffect : CardEffect
    {
        [SerializeField] private bool _avoidDuplicateTargets;

        public bool AvoidDuplicateTargets => _avoidDuplicateTargets;

        public override void Execute(EffectContext context)
        {
            int repeatCount = context.CardData.Values[context.ValueIndex] - 1;
            int previousIndex = context.ValueIndex - 1;

            if (previousIndex < 0 || previousIndex >= context.CurrentEffectsList.Count)
                return;

            var previousEffect = context.CurrentEffectsList[previousIndex];

            int savedIndex = context.ValueIndex;
            var savedUsedTargets = context.UsedTargets;

            // Если CardCastService уже завёл набор (в карте есть эта галочка) — переиспользуем его,
            // тогда учитываются и цели, выбранные первым применением эффекта.
            if (_avoidDuplicateTargets && savedUsedTargets == null)
                context.UsedTargets = new HashSet<CardAndHealthEntityOwnerData>();

            context.ValueIndex = previousIndex;

            try
            {
                for (int i = 0; i < repeatCount; i++)
                    previousEffect.Execute(context);
            }
            finally
            {
                context.ValueIndex = savedIndex;
                context.UsedTargets = savedUsedTargets;
            }
        }
    }
}
