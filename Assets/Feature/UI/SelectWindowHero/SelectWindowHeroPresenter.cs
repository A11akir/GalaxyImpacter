using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Data;
using Feature.GameSessionData;
using Feature.GameSessionFSM;
using Feature.Hero;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroPresenter
    {
        public SelectWindowHeroView _selectWindowHeroView;
        private SelectWindowHeroModel _selectWindowHeroModel;
        private GameSessionFSM.GameSessionFSM  _gameSessionFSM;
        private GameSessionData.GameSessionData _gameSessionData;
        
        private PickStateGameSessionFSM _pickStateGameSessionFSM;
        private GameData _gameData;
        public event Action OnPickedHero;
        
        public SelectWindowHeroPresenter(SelectWindowHeroView selectWindowHeroView, GameData gameData, SelectWindowHeroModel selectWindowHeroModel, GameSessionFSM.GameSessionFSM gameSessionFsm, GameSessionData.GameSessionData gameSessionData)
        {
            _selectWindowHeroView = selectWindowHeroView;
            _gameData = gameData;
            _selectWindowHeroModel = selectWindowHeroModel;
            _gameSessionFSM = gameSessionFsm;
            _gameSessionData = gameSessionData;

            _selectWindowHeroView.OnBanHeroButtonClicked += BanHero;
            _selectWindowHeroView.OnChoseHeroButtonClicked += ChoseHeroPlayer;
            _selectWindowHeroView.OnSelectWindowHeroView += SelectHero;
        }

        private void SetViewSelectedHeroBot()
        {
            _selectWindowHeroView._selectHeroView.WasSetHeroEnemy();
        }

        public void SelectHero()
        {
            _selectWindowHeroModel._selectedHero =
                _selectWindowHeroView._selectHeroView.HeroData;
        }
        public List<GameSessionPlayerData> GetCurrentHeroStats()
            => _selectWindowHeroModel._heroesForChose;
        public void BanHero()
        {
            var selectedHero = _selectWindowHeroView._selectHeroView;
            
            selectedHero.BanHero();
            
            RemoveHeroFromList(selectedHero);
            
            _selectWindowHeroView.ClearAllSelected();
            _gameSessionFSM.SetState<PickStateGameSessionFSM>();
        }
        private void RemoveHeroFromList(HeroView selectedHero)
        {
            var heroToRemove = _selectWindowHeroModel._heroesForChose
                .FirstOrDefault(h => h._heroName == selectedHero.HeroData._heroName);
            
            if (heroToRemove != null)
            {
                _selectWindowHeroModel._heroesForChose.Remove(heroToRemove);
                _selectWindowHeroView.heroViews.Remove(selectedHero);
            }
        }

        private void ChoseHeroPlayer()
        {
            var selectedHero = _selectWindowHeroView._selectHeroView;
            
            SetViewSelectedHeroBot();
            
            RemoveHeroFromList(selectedHero);

            _selectWindowHeroView.ClearAllSelected();

            _gameSessionData.PlayerHero = _selectWindowHeroModel._selectedHero;

            
            _selectWindowHeroView.HideSelectButton();
            _selectWindowHeroView.OnChoseHeroButtonClicked -= ChoseHeroPlayer;
            OnPickedHero?.Invoke();
            if (_gameSessionData.PlayersHaveHero())
                _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();
            
            selectedHero._isBanned = true;
        }
        public void ChoseHeroEnemy()
        {
            var selectedHero = _selectWindowHeroView._selectHeroView;
            
            
            
            SetViewSelectedHeroBot();
            
            _selectWindowHeroView.ClearAllSelected();
            RemoveHeroFromList(selectedHero);
            
            _gameSessionData.EnemyHero = _selectWindowHeroView._selectHeroView.HeroData;

            if (_gameSessionData.PlayersHaveHero())
                _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();

            selectedHero._isBanned = true;
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
                    _heroPowerCost = selectedHeroStats.HeroPowerCost,
                    
                    _iconImage = selectedHeroStats.IconImage
                };

                _selectWindowHeroModel._heroesForChose.Add(heroData);
                availableHeroes.RemoveAt(randomIndex);
            }
        }

        public void SetActive() => _selectWindowHeroView.gameObject.SetActive(true);
        public void SetInactive() => _selectWindowHeroView.gameObject.SetActive(false);

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
                var data = _selectWindowHeroModel._heroesForChose[i];
                _selectWindowHeroView.heroViews[i].SetData(data);
            }
        }
    }
}