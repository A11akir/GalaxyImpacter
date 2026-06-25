// HeroClassPurchaseCount.cs — обновлённый с учётом редкости
using System.Collections.Generic;
using Feature.Card.Script;

namespace Feature.Hero
{
    public class HeroClassPurchaseCount
    {
        private readonly Dictionary<(AllHeroClass, CardRarity), int> _purchasedCounts = new();

        public void AddPurchase(AllHeroClass heroClass, CardRarity rarity)
        {
            var key = (heroClass, rarity);
            if (!_purchasedCounts.ContainsKey(key))
                _purchasedCounts[key] = 0;
            _purchasedCounts[key]++;
        }

        public int GetPurchaseCount(AllHeroClass heroClass, CardRarity rarity)
        {
            return _purchasedCounts.TryGetValue((heroClass, rarity), out var count) ? count : 0;
        }
    }
}