// CardEffect.cs — базовый класс получает общую логику выбора целей
using System;
using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public abstract class CardEffect
    {
        [SerializeField] protected TargetSelectionType TargetSelection = TargetSelectionType.Target;
        [SerializeField] protected bool RandomSingleTarget;

        [SerializeReference] protected List<ITargetModifier> _targetModifiers = new();

        public abstract void Execute(EffectContext context);

        /// <summary>
        /// Сколько идущих за этим эффектом эффектов он оборачивает и выполняет сам.
        /// Основной цикл пропускает их, чтобы они не срабатывали дважды. У обычных эффектов — 0.
        /// </summary>
        public virtual int ControlledFollowingCount => 0;

        /// <summary>
        /// Выполняет count эффектов, идущих сразу после текущего, передавая каждому
        /// его собственный ValueIndex (значение из Values по его позиции) и
        /// восстанавливая исходный ValueIndex после цикла.
        /// </summary>
        protected void ExecuteFollowingEffects(EffectContext context, int count)
        {
            var effects = context.CurrentEffectsList;
            if (effects == null) return;

            int savedIndex = context.ValueIndex;

            for (int offset = 1; offset <= count; offset++)
            {
                int index = savedIndex + offset;
                if (index < 0 || index >= effects.Count) break;

                var effect = effects[index];
                if (effect == null) continue;

                ExecuteInSlot(context, offset, effect.Execute);
            }
        }

        /// <summary>
        /// Выполняет вложенный эффект в его слоте Values и возвращает контекст в исходное
        /// состояние. Единое правило: контейнер занимает слот N, вложенный эффект —
        /// N + 1 (offset), следующий за ним — N + 2 и так далее.
        /// </summary>
        protected void ExecuteInSlot(EffectContext context, int offset, Action<EffectContext> body)
        {
            int savedIndex = context.ValueIndex;
            var savedTarget = context.Target;
            var savedTargetCard = context.TargetCard;

            context.ValueIndex = savedIndex + offset;
            try
            {
                body(context);
            }
            finally
            {
                context.ValueIndex = savedIndex;
                context.Target = savedTarget;
                context.TargetCard = savedTargetCard;
            }
        }

        /// <summary>
        /// Единый проход по плоскому списку эффектов. Каждый эффект читает Values по
        /// своему индексу; эффекты-контейнеры сами выполняют следующие за ними эффекты,
        /// поэтому ControlledFollowingCount эффектов после контейнера пропускаются.
        /// </summary>
        public static void ExecuteAll(EffectContext context, IReadOnlyList<CardEffect> effects)
        {
            if (effects == null) return;

            for (int i = 0; i < effects.Count; i++)
            {
                var effect = effects[i];
                if (effect == null) continue;

                context.ValueIndex = i;
                effect.Execute(context);
                i += effect.ControlledFollowingCount;
            }
        }

        protected List<CardAndHealthEntityOwnerData> ResolveTargets(EffectContext ctx)
        {
            var pool = GetTargetsPool(ctx);
            
            pool = RemoveRepeatTargetIfNeed(ctx, pool);

            pool = GetRandomTargetIfNeed(pool);

            pool = ApplyTargetModifiers(ctx, pool);

            SaveUsedTargets(ctx, pool);

            return pool;
        }

        private List<CardAndHealthEntityOwnerData> ApplyTargetModifiers(EffectContext ctx, List<CardAndHealthEntityOwnerData> pool)
        {
            for (int m = 0; m < _targetModifiers.Count; m++)
            {
                var modifier = _targetModifiers[m];
                if (modifier == null) continue;

                int offset = m;
                ExecuteInSlot(ctx, 1 + offset, c => pool = modifier.ModifyTargets(pool, c));
            }

            return pool;
        }

        private List<CardAndHealthEntityOwnerData> GetRandomTargetIfNeed(List<CardAndHealthEntityOwnerData> pool)
        {
            if (RandomSingleTarget && pool.Count > 0)
                pool = new List<CardAndHealthEntityOwnerData> { pool[UnityEngine.Random.Range(0, pool.Count)]};
            return pool;
        }

        private static void SaveUsedTargets(EffectContext ctx, List<CardAndHealthEntityOwnerData> pool)
        {
            if (ctx.UsedTargets != null)
            {
                foreach (var target in pool)
                    ctx.UsedTargets.Add(target);
            }
        }

        private static List<CardAndHealthEntityOwnerData> RemoveRepeatTargetIfNeed(EffectContext ctx, List<CardAndHealthEntityOwnerData> pool)
        {
            if (ctx.UsedTargets != null && ctx.UsedTargets.Count > 0)
                pool = pool.Where(target => target != null && !ctx.UsedTargets.Contains(target)).ToList();
            return pool;
        }

        protected List<CardAndHealthEntityOwnerData> GetTargetsPool(EffectContext ctx) =>
            TargetScopeResolver.Resolve(TargetSelection, ctx.Caster, ctx.Target, ctx.GameSessionModel);
    }
}