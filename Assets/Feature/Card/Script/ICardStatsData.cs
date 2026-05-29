using System.Collections.Generic;
using Feature.GoogleSheets;
using Feature.Hero;
using UnityEngine;

namespace Feature.Card.Script
{
    public interface ICardStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        CardRarity Rarity { get; set; }
        List<AllHeroClass> Specialization { get; set; }
        int Level { get; set; }
        Sprite IconImage { get; set; }
        bool IsHero { get; }
    }
}