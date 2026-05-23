using System.Collections.Generic;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Card.Script
{
    public interface ICardStatsData
    {
        string Name { get; set; }
        int Cost { get; set; }
        CardRarity Rarity { get; set; }
        List<string> Specialization { get; set; }
        int Level { get; set; }
        Sprite IconImage { get; set; }
        bool IsHero { get; }
    }
}