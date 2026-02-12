using System.Collections.Generic;
using Feature.Data;
using Feature.GameSessionData;
using Feature.Hero;
using UnityEngine;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroPresenter
    {
        private SelectWindowHeroView _selectWindowHeroView;
        private SelectWindowHeroModel _selectWindowHeroModel;
        
        
        private GameData _gameData;
        
        
        public SelectWindowHeroPresenter(SelectWindowHeroView selectWindowHeroView, GameData gameData, SelectWindowHeroModel selectWindowHeroModel)
        {
            _selectWindowHeroView = selectWindowHeroView;
            _gameData = gameData;
            _selectWindowHeroModel = selectWindowHeroModel;

            _selectWindowHeroView.OnBanHeroButtonClicked += BanHero;
            _selectWindowHeroView.OnChoseHeroButtonClicked += ChoseHero;
            
        }

        private void BanHero()
        {
            
        }

        private void ChoseHero()
        {
            
        }

        public void SelectRandomHeroes()
        {
            
            List<HeroStatsData> availableHeroes = new List<HeroStatsData>(_gameData.allHeroStats);
            
            int heroesToSelect = Mathf.Min(_selectWindowHeroModel.countPersonForChose, availableHeroes.Count);
    
            for (int i = 0; i < heroesToSelect; i++)
            {
                int randomIndex = Random.Range(0, availableHeroes.Count);
                HeroStatsData selectedHeroStats = availableHeroes[randomIndex];
                
                GameSessionPlayerData heroData = new GameSessionPlayerData
                {
                    _heroName = selectedHeroStats.Name,
                    _health = selectedHeroStats.Health,
                    /*_heroPowerData = selectedHeroStats.HeroPowerCost*/
                };

                _selectWindowHeroModel._heroesForChose.Add(heroData);

                availableHeroes.RemoveAt(randomIndex);
            }
            
        }

        public void SetActive()
        {
            _selectWindowHeroView.gameObject.SetActive(true);
        }

        public void SetBanMode()
        {
            _selectWindowHeroView.buttonBanHero.gameObject.SetActive(true);
            _selectWindowHeroView.buttonSelectHero.gameObject.SetActive(false);
        }

        public void SetSelectMode()
        {
            _selectWindowHeroView.buttonBanHero.gameObject.SetActive(false);
            _selectWindowHeroView.buttonSelectHero.gameObject.SetActive(true);
        }

        public void SetRandomHeroes()
        {
            for (int i = 0; i < _selectWindowHeroModel._heroesForChose.Count; i++)
            {
                _selectWindowHeroView.heroViews[i]._nameText.text = _selectWindowHeroModel._heroesForChose[i]._heroName;
                _selectWindowHeroView.heroViews[i]._healthText.text = _selectWindowHeroModel._heroesForChose[i]._health.ToString();
                /*_selectWindowHeroView.heroViews[i]._healthText.text = _selectWindowHeroModel._heroesForChose[i]._heroPowerData.ToString();*/
            }
        }
    }
}