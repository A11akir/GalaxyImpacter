using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    public static class TargetScopeResolver
    {
        public static List<CardAndHealthEntityOwnerData> Resolve(
            TargetSelectionType scope,
            CardAndHealthEntityOwnerData self,
            CardAndHealthEntityOwnerData target,
            GameSessionModel model)
        {
            if (model == null) return new List<CardAndHealthEntityOwnerData>();

            var side = model.GetPlayerDataByOwner(self);
            bool selfIsPlayer = side == model.PlayerHero;
            var ally  = selfIsPlayer ? model.PlayerHero : model.EnemyHero;
            var enemy = selfIsPlayer ? model.EnemyHero : model.PlayerHero;

            return scope switch
            {
                TargetSelectionType.Target       => new() { target },
                TargetSelectionType.Self         => new() { self },
                TargetSelectionType.PlayerHero   => new() { model.PlayerHero.MainHeroEntity() },
                TargetSelectionType.EnemyHero    => new() { model.EnemyHero.MainHeroEntity() },
                TargetSelectionType.EnemyMinion  => enemy.Minions.ToList(),
                TargetSelectionType.PlayerMinion => ally.Minions.ToList(),
                TargetSelectionType.Allies       => ally.CardAndHealthEntityOwners.ToList(),
                TargetSelectionType.Enemies      => enemy.CardAndHealthEntityOwners.ToList(),
                TargetSelectionType.All          => model.GetAllEntityOwners().ToList(),
                _                                => new List<CardAndHealthEntityOwnerData>()
            };
        }
    }
}