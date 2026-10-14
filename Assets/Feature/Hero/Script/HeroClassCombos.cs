using System.Collections.Generic;

namespace Feature.Hero.Script
{
    public static class HeroClassCombos
    {
        private static readonly Dictionary<AllHeroClass, HashSet<AllHeroClass>> Combos =
            new Dictionary<AllHeroClass, HashSet<AllHeroClass>>
            {
                { AllHeroClass.LightningMage,      new HashSet<AllHeroClass> { AllHeroClass.FireMage, AllHeroClass.WindMage } },
                { AllHeroClass.MetalMage,          new HashSet<AllHeroClass> { AllHeroClass.FireMage, AllHeroClass.EarthMage } },
                { AllHeroClass.AbyssLord,          new HashSet<AllHeroClass> { AllHeroClass.WaterMage, AllHeroClass.Explorer } },
                { AllHeroClass.TimeMage,           new HashSet<AllHeroClass> { AllHeroClass.WindMage, AllHeroClass.Explorer } },
                { AllHeroClass.Witcher,            new HashSet<AllHeroClass> { AllHeroClass.Assassin, AllHeroClass.Alchemist } },
                { AllHeroClass.Dragonborn,         new HashSet<AllHeroClass> { AllHeroClass.Warrior, AllHeroClass.FireMage } },
                { AllHeroClass.GravityMage,        new HashSet<AllHeroClass> { AllHeroClass.EarthMage, AllHeroClass.WindMage } },
                { AllHeroClass.SupremeAlchemist,   new HashSet<AllHeroClass> { AllHeroClass.Alchemist, AllHeroClass.Explorer, AllHeroClass.Monster } },
                { AllHeroClass.InvincibleWanderer, new HashSet<AllHeroClass> { AllHeroClass.Warrior, AllHeroClass.Assassin, AllHeroClass.Explorer } },
                { AllHeroClass.AbsolutePredator,   new HashSet<AllHeroClass> { AllHeroClass.Warrior, AllHeroClass.Assassin, AllHeroClass.Monster } },
                { AllHeroClass.DeathKing,          new HashSet<AllHeroClass> { AllHeroClass.WaterMage, AllHeroClass.Warrior, AllHeroClass.Monster, AllHeroClass.Alchemist } },
                { AllHeroClass.Avatar,             new HashSet<AllHeroClass> { AllHeroClass.WindMage, AllHeroClass.WaterMage, AllHeroClass.FireMage, AllHeroClass.EarthMage } },
            };

        public static bool Matches(AllHeroClass requiredClass, List<AllHeroClass> cardSpecialization)
        {
            if (requiredClass == AllHeroClass.All)
                return true;

            if (cardSpecialization == null)
                return false;

            if (Combos.TryGetValue(requiredClass, out var baseSet))
                return baseSet.SetEquals(cardSpecialization);

            return cardSpecialization.Contains(requiredClass);
        }
    }
}