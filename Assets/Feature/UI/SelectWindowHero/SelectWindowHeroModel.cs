using System.Collections.Generic;
using Feature.Data;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroModel
    {
        public GameSessionPlayerData _selectedHero;
        
        public List<GameSessionPlayerData> _heroesForChose = new List<GameSessionPlayerData>();

        private int countPersonForChose = 5;
        private GameData _gameData;

        public SelectWindowHeroModel(GameData gameData) => _gameData = gameData;

        public void SelectStartRandomHeroes()
        {
            List<HeroStatsData> availableHeroes = new List<HeroStatsData>(_gameData.allHeroStats);
            
            int heroesToSelect = Mathf.Min(countPersonForChose, availableHeroes.Count);
    
            for (int i = 0; i < heroesToSelect; i++)
            {
                int randomIndex = Random.Range(0, availableHeroes.Count);
                HeroStatsData selectedHeroStats = availableHeroes[randomIndex];
                
                GameSessionPlayerData heroData = new GameSessionPlayerData
                {
                    _heroName = selectedHeroStats.Name,
                    _health = selectedHeroStats.Health,
                    _heroPowerCost = selectedHeroStats.HeroPowerCost,
                    _iconImage = selectedHeroStats.IconImage
                };

                _heroesForChose.Add(heroData);
                availableHeroes.RemoveAt(randomIndex);
            }
        }

    }
}