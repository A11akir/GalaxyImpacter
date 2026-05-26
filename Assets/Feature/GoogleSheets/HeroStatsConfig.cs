using System.Collections.Generic;
using Feature.Card.Script;

namespace Feature.GoogleSheets
{
    public class HeroStatsConfig
    {
        public string HeroName;
        public int Health;
        public int HeroPowerCost;
    }

    public class ItemStatsConfig
    {
        public string ItemName;
        public string Description;
        public int GoldCost;
        public List<int> Values;
    }
    
    [System.Serializable]
    public class SpellStatsConfig
    {
        public string Name;
        public int Cost;
        public List<int> Values;
        public string Description;
        public string Rarity;
        public List<string> Specialization;
        public int Level;
        public string MinionNameOwner { get; set; }
        public TargetType TargetType { get; set; } = TargetType.All;

        public bool InCollection { get; set; }
    }
    
    [System.Serializable]
    public class MinionStatsConfig
    {
        public string Name;
        public int Cost;
        public List<int> Values;
        public List<ISpellStatsData> SpellsList;
        public List<string> SpellNames = new List<string>();
        public string Rarity;
        public List<string> Specialization;
        public int Level;
        public int Health;
        public int Chakra;
        public int HandCardCount;

        public bool InCollection { get; set; }
        public TargetType TargetType { get; set; } = TargetType.BoardPlace;
    }
}