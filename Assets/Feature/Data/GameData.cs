using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.Hero;
using Feature.Items.Scripts;
using UnityEngine;

namespace Feature.Data
{
    [CreateAssetMenu(
        fileName = "HeroStatsData",
        menuName = "Configs/HeroAndCardData",
        order = 1)]
    public class GameData : ScriptableObject
    {
        public List<HeroStatsData> allHeroStats = new List<HeroStatsData>();
        public List<CardStatsData> allCards = new List<CardStatsData>();
        public List<ItemData> allItems = new List<ItemData>();
    }
}