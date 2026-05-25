// HeroClassData.cs
using System.Collections.Generic;

namespace Feature.Hero
{
    public class HeroClassData
    {
        public AllHeroClass MainClass { get; private set; }

        private readonly List<AllHeroClass> _classes = new();
        public IReadOnlyList<AllHeroClass> Classes => _classes;

        public void SetMainClass(AllHeroClass heroClass)
        {
            MainClass = heroClass;
            AddClass(heroClass);
        }

        public void AddClass(AllHeroClass heroClass)
        {
            if (!_classes.Contains(heroClass))
                _classes.Add(heroClass);
        }

        public bool HasClass(AllHeroClass heroClass) => _classes.Contains(heroClass);
    }
}