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

                context.ValueIndex = index;
                effects[index]?.Execute(context);
            }

            context.ValueIndex = savedIndex;
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
            var pool = GetCandidatePool(ctx);
            

            // Галочка «не выбирать одну и ту же цель дважды» (RepeatEffect):
            // цели, уже получившие эффект в этой серии повторов, исключаются из пула.
            if (ctx.UsedTargets != null && ctx.UsedTargets.Count > 0)
                pool = pool.Where(target => target != null && !ctx.UsedTargets.Contains(target)).ToList();

            var resolved = pool;

            if (RandomSingleTarget && pool.Count > 0)
                resolved = new List<CardAndHealthEntityOwnerData> { pool[UnityEngine.Random.Range(0, pool.Count)] };

            if (ctx.UsedTargets != null)
            {
                foreach (var target in resolved)
                    ctx.UsedTargets.Add(target);
            }

            return resolved;
        }

        private List<CardAndHealthEntityOwnerData> GetCandidatePool(EffectContext ctx)
        {
            var casterSide = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster);
            bool casterIsPlayer = casterSide == ctx.GameSessionModel.PlayerHero;

            var ally = casterIsPlayer ? ctx.GameSessionModel.PlayerHero : ctx.GameSessionModel.EnemyHero;
            var enemy = casterIsPlayer ? ctx.GameSessionModel.EnemyHero : ctx.GameSessionModel.PlayerHero;

            return TargetSelection switch
            {
                TargetSelectionType.Target       => new List<CardAndHealthEntityOwnerData> { ctx.Target },
                TargetSelectionType.Self         => new List<CardAndHealthEntityOwnerData> { ctx.Caster },
                TargetSelectionType.PlayerHero    => new List<CardAndHealthEntityOwnerData> { ctx.GameSessionModel.PlayerHero.MainHeroEntity() },
                TargetSelectionType.EnemyHero     => new List<CardAndHealthEntityOwnerData> { ctx.GameSessionModel.EnemyHero.MainHeroEntity() },
                TargetSelectionType.EnemyMinion   => enemy.AliveMinions.ToList(),
                TargetSelectionType.PlayerMinion  => ally.AliveMinions.ToList(),
                TargetSelectionType.Allies        => ally.AllAlive.ToList(),
                TargetSelectionType.Enemies       => enemy.AllAlive.ToList(),
                TargetSelectionType.All           => ctx.GameSessionModel.GetAllEntityOwners().Where(o => o.HealthValue > 0).ToList(),
                _ => new List<CardAndHealthEntityOwnerData>()
            };
        }
    }
}