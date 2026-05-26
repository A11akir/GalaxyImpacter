using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Feature.Hero
{
    
    public class HeroClassLevel
    {
        private readonly Dictionary<AllHeroClass, ReactiveProperty<int>> _classLevels = new();

        public ReadOnlyReactiveProperty<int> GetLevel(AllHeroClass heroClass)
        {
            EnsureExists(heroClass);
            return _classLevels[heroClass];
        }

        public void RecalculateFromDeck(IEnumerable<CardStatsData> deck)
        {
            foreach (var key in _classLevels.Keys)
                _classLevels[key].Value = 0;

            foreach (var card in deck)
            {
                if (card?.Specialization == null) continue;

                foreach (var spec in card.Specialization)
                {
                    if (!System.Enum.TryParse<AllHeroClass>(spec, out var heroClass)) continue;
                    if (heroClass == AllHeroClass.All) continue;

                    EnsureExists(heroClass);
                    _classLevels[heroClass].Value++;
                }
            }
        }

        private void EnsureExists(AllHeroClass heroClass)
        {
            if (!_classLevels.ContainsKey(heroClass))
                _classLevels[heroClass] = new ReactiveProperty<int>(0);
        }
    }
}