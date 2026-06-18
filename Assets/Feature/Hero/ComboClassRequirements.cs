using System.Collections.Generic;

namespace Feature.Hero
{
    public static class ComboClassRequirements
    {
        public static readonly IReadOnlyDictionary<ComboHeroClass, IReadOnlyList<BaseHeroClass>> Requirements =
            new Dictionary<ComboHeroClass, IReadOnlyList<BaseHeroClass>>
            {
                // 4-компонентные комбо
                { ComboHeroClass.Avatar,              new[] { BaseHeroClass.WaterMage, BaseHeroClass.EarthMage, BaseHeroClass.FireMage, BaseHeroClass.WindMage } },
                { ComboHeroClass.DeathKing,           new[] { BaseHeroClass.Monster,   BaseHeroClass.WaterMage, BaseHeroClass.Warrior,  BaseHeroClass.Alchemist } },

                // 3-компонентные комбо
                { ComboHeroClass.AbsolutePredator,    new[] { BaseHeroClass.Monster,   BaseHeroClass.Assassin,  BaseHeroClass.Warrior } },
                { ComboHeroClass.InvincibleWanderer,  new[] { BaseHeroClass.Warrior,   BaseHeroClass.Assassin,  BaseHeroClass.Explorer } },
                { ComboHeroClass.SupremeAlchemist,    new[] { BaseHeroClass.Monster,   BaseHeroClass.Alchemist, BaseHeroClass.Explorer } },

                // 2-компонентные комбо
                { ComboHeroClass.LightningMage,       new[] { BaseHeroClass.FireMage,  BaseHeroClass.WindMage } },
                { ComboHeroClass.MetalMage,           new[] { BaseHeroClass.EarthMage, BaseHeroClass.FireMage } },
                { ComboHeroClass.AbyssLord,           new[] { BaseHeroClass.WaterMage, BaseHeroClass.Explorer } },
                { ComboHeroClass.TimeMage,            new[] { BaseHeroClass.WindMage,  BaseHeroClass.Explorer } },
                { ComboHeroClass.Witcher,             new[] { BaseHeroClass.Assassin,  BaseHeroClass.Alchemist } },
                { ComboHeroClass.Dragonborn,          new[] { BaseHeroClass.FireMage,  BaseHeroClass.Warrior } },
                { ComboHeroClass.GravityMage,         new[] { BaseHeroClass.WindMage,  BaseHeroClass.EarthMage } },
            };

        public static AllHeroClass ToAllHeroClass(ComboHeroClass combo) => combo switch
        {
            ComboHeroClass.LightningMage      => AllHeroClass.LightningMage,
            ComboHeroClass.MetalMage          => AllHeroClass.MetalMage,
            ComboHeroClass.AbyssLord          => AllHeroClass.AbyssLord,
            ComboHeroClass.TimeMage           => AllHeroClass.TimeMage,
            ComboHeroClass.Witcher            => AllHeroClass.Witcher,
            ComboHeroClass.Dragonborn         => AllHeroClass.Dragonborn,
            ComboHeroClass.GravityMage        => AllHeroClass.GravityMage,
            ComboHeroClass.SupremeAlchemist   => AllHeroClass.SupremeAlchemist,
            ComboHeroClass.InvincibleWanderer => AllHeroClass.InvincibleWanderer,
            ComboHeroClass.AbsolutePredator   => AllHeroClass.AbsolutePredator,
            ComboHeroClass.DeathKing          => AllHeroClass.DeathKing,
            ComboHeroClass.Avatar             => AllHeroClass.Avatar,
            _                                 => AllHeroClass.All
        };

        public static AllHeroClass ToAllHeroClass(BaseHeroClass baseClass) => baseClass switch
        {
            BaseHeroClass.Alchemist => AllHeroClass.Alchemist,
            BaseHeroClass.Assassin  => AllHeroClass.Assassin,
            BaseHeroClass.EarthMage => AllHeroClass.EarthMage,
            BaseHeroClass.Explorer  => AllHeroClass.Explorer,
            BaseHeroClass.FireMage  => AllHeroClass.FireMage,
            BaseHeroClass.Monster   => AllHeroClass.Monster,
            BaseHeroClass.Warrior   => AllHeroClass.Warrior,
            BaseHeroClass.WaterMage => AllHeroClass.WaterMage,
            BaseHeroClass.WindMage  => AllHeroClass.WindMage,
            _                       => AllHeroClass.All
        };

        public static bool IsUnlocked(HeroClassData classData, ComboHeroClass combo)
        {
            if (!Requirements.TryGetValue(combo, out var required)) return false;

            foreach (var baseClass in required)
            {
                if (!classData.HasClass(ToAllHeroClass(baseClass)))
                    return false;
            }

            return true;
        }
    }
}
