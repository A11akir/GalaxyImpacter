    using System.Collections.Generic;
    using Feature.Data;
    using Feature.GameSessionData;
    using Feature.GoogleSheets;
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
                        _iconImage = selectedHeroStats.IconImage,
                        HeroPowers = CopyHeroPowers(selectedHeroStats.HeroPowers)
                    };


                    heroData.MainHeroEntity()._heroName = selectedHeroStats.Name;
                    heroData.MainHeroEntity().HealthValue = selectedHeroStats.Health;
                    heroData.MainHeroEntity().MaxHealth = selectedHeroStats.Health;

                    if (System.Enum.TryParse<AllHeroClass>(selectedHeroStats.Name, out var heroClass))
                        heroData.HeroClassData.SetMainClass(heroClass);
                    _heroesForChose.Add(heroData);
                    availableHeroes.RemoveAt(randomIndex);
                }
            }

            // Копии силы героя на игрока: цена меняется эффектами, общий ассет менять нельзя.
            private static List<SpellCardData> CopyHeroPowers(List<SpellCardData> heroPowers)
            {
                var copies = new List<SpellCardData>();

                if (heroPowers == null) return copies;

                foreach (var heroPower in heroPowers)
                {
                    var copy = ScriptableObject.Instantiate(heroPower);
                    copy.id = System.Guid.NewGuid().ToString();
                    copies.Add(copy);
                }

                return copies;
            }
        }
    }