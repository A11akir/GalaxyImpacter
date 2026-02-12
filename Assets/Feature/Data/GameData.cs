using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.Hero;
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
        public List<GameplayCard> allCards = new List<GameplayCard>();
    }
}