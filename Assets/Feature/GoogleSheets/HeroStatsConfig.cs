using System.Collections.Generic;

namespace Feature.GoogleSheets
{
    public class HeroStatsConfig
    {
        public string HeroName;
        public int Health;
        public int HeroPowerCost;
    }
    public class CardStatsConfig
    {
        public string Name;
        public int Cost;
        public int Value1;
        public int Value2;
        public int Value3;
        public string Description;
        public string Rarity;
        public List<string> Specialization;
        public int Level;
    }
}