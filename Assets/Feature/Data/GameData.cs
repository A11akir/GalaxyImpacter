using System.Collections.Generic;
using Feature.Card.Script;
using Feature.Hero;
using Feature.Items.Scripts;
using UnityEngine;

namespace Feature.Data
{
    [CreateAssetMenu(
        fileName = "GameData",
        menuName = "Configs/GameData",
        order = 1)]
    public class GameData : ScriptableObject
    {
        [Header("Heroes")]
        public List<HeroStatsData> allHeroStats = new List<HeroStatsData>();
        
        [Header("Items")]
        public List<ItemData> allItems = new List<ItemData>();
        
        [Header("BaseCards")]
        public List<CardStatsData> baseCards = new List<CardStatsData>();
        
        [Header("Cards by Base Class")]
        public List<CardStatsData> alchemistCards = new List<CardStatsData>();
        public List<CardStatsData> assassinCards = new List<CardStatsData>();
        public List<CardStatsData> earthMageCards = new List<CardStatsData>();
        public List<CardStatsData> explorerCards = new List<CardStatsData>();
        public List<CardStatsData> fireMageCards = new List<CardStatsData>();
        public List<CardStatsData> monsterCards = new List<CardStatsData>();
        public List<CardStatsData> warriorCards = new List<CardStatsData>();
        public List<CardStatsData> waterMageCards = new List<CardStatsData>();
        public List<CardStatsData> windMageCards = new List<CardStatsData>();
        
        [Header("Cards by Combo Class")]
        public List<CardStatsData> lightningMageCards = new List<CardStatsData>();
        public List<CardStatsData> metalMageCards = new List<CardStatsData>();
        public List<CardStatsData> abyssLordCards = new List<CardStatsData>();
        public List<CardStatsData> timeMageCards = new List<CardStatsData>();
        public List<CardStatsData> witcherCards = new List<CardStatsData>();
        public List<CardStatsData> dragonbornCards = new List<CardStatsData>();
        public List<CardStatsData> gravityMageCards = new List<CardStatsData>();
        public List<CardStatsData> supremeAlchemistCards = new List<CardStatsData>();
        public List<CardStatsData> invincibleWandererCards = new List<CardStatsData>();
        public List<CardStatsData> absolutePredatorCards = new List<CardStatsData>();
        public List<CardStatsData> deathKingCards = new List<CardStatsData>();
        public List<CardStatsData> avatarCards = new List<CardStatsData>();
        
        [Header("All Cards")]
        public List<CardStatsData> allCards = new List<CardStatsData>();
        
        public List<CardStatsData> GetCardsByHeroName(string heroName)
        {
            return heroName switch
            {
                "Alchemist"  => alchemistCards,
                "Assassin"   => assassinCards,
                "EarthMage"  => earthMageCards,
                "Explorer"   => explorerCards,
                "FireMage"   => fireMageCards,
                "Monster"    => monsterCards,
                "Warrior"    => warriorCards,
                "WaterMage"  => waterMageCards,
                "WindMage"   => windMageCards,
                _ => allCards
            };
        }
    }
}