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