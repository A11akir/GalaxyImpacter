using System;
using System.Linq;
using Feature.GameSessionData;
using Feature.GameSessionFSM;
using UnityEngine;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroPresenter
    {
        public SelectWindowHeroView _selectWindowHeroView;
        private SelectWindowHeroModel _selectWindowHeroModel;
        private GameSessionFSM.GameSessionFSM  _gameSessionFSM;
        private GameSessionModel _gameSessionModel;
        
        private PickStateGameSessionFSM _pickStateGameSessionFSM;
        public event Action OnPlayerPickedHero;
        
        public SelectWindowHeroPresenter(SelectWindowHeroView selectWindowHeroView,
            SelectWindowHeroModel selectWindowHeroModel, GameSessionFSM.GameSessionFSM gameSessionFsm,
            GameSessionModel gameSessionModel)
        {
            _selectWindowHeroView = selectWindowHeroView;
            _selectWindowHeroModel = selectWindowHeroModel;
            _gameSessionFSM = gameSessionFsm;
            _gameSessionModel = gameSessionModel;

            _selectWindowHeroView.OnBanHeroButtonClicked += BanHero;
            _selectWindowHeroView.OnChoseHeroButtonClicked += ChoseSelectedHeroPlayer;
            _selectWindowHeroView.OnSelectWindowHeroView += SelectHero;

            _selectWindowHeroView.Init();
        }

        private void SetViewChosedHeroBot()
        {
            if (_selectWindowHeroView._selectHeroView == null) return;
            _selectWindowHeroView._selectHeroView.WasSetHeroEnemy();
        }

        public void SelectHero()
        {
            _selectWindowHeroModel._selectedHero =
                _selectWindowHeroView._selectHeroView.HeroData;
        }

        public void BanHero()
        {
            if (_selectWindowHeroView._selectHeroView == null) return;
            var selectedHero = _selectWindowHeroView._selectHeroView;
            
            selectedHero.BanHeroView();
            
            RemoveHeroFromSelectList(selectedHero);
            
            _selectWindowHeroView.ClearAllViewSelected();
            ClearSelectedHero();
            _gameSessionFSM.SetState<PickStateGameSessionFSM>();
        }
        
        private void RemoveHeroFromSelectList(HeroView selectedHero)
        {
            if (_selectWindowHeroModel._heroesForChose == null) return;
            if (selectedHero == null) return;
            
            var heroToRemove = _selectWindowHeroModel._heroesForChose
                .FirstOrDefault(h => h.MainHeroEntity()._heroName == selectedHero.HeroData.MainHeroEntity()._heroName);
            
            if (heroToRemove != null)
            {
                _selectWindowHeroModel._heroesForChose.Remove(heroToRemove);
                _selectWindowHeroView.heroViews.Remove(selectedHero);
            }
        }

        public void ChoseSelectedHeroPlayer() => CompleteHeroSelection(true);
        public void ChoseSelectedHeroEnemy() => CompleteHeroSelection(false);

        private void CompleteHeroSelection(bool isPlayer)
        {
            if (_selectWindowHeroView._selectHeroView == null) return;
            var selectedHero = _selectWindowHeroView._selectHeroView;
    
            SetViewChosedHeroBot();
            RemoveHeroFromSelectList(selectedHero);
            _selectWindowHeroView.ClearAllViewSelected();
    
            if (isPlayer)
            {
                _gameSessionModel.PlayerHero = _selectWindowHeroModel._selectedHero;
                _selectWindowHeroView.HideSelectButton();
                _selectWindowHeroView.OnChoseHeroButtonClicked -= ChoseSelectedHeroPlayer;
                OnPlayerPickedHero?.Invoke();
            }
            else _gameSessionModel.EnemyHero = _selectWindowHeroView._selectHeroView.HeroData;
    
            selectedHero._isBlockedForSelect = true;
            
            ClearSelectedHero();
            
            if (_gameSessionModel.PlayersHaveHero())
                _gameSessionFSM.SetState<PrepareStateGameSessionFSM>();
        }

        private void ClearSelectedHero()
        {
            _selectWindowHeroView._selectHeroView = null;
            _selectWindowHeroModel._selectedHero = null;
        }

        public void SelectStartRandomHeroes() => _selectWindowHeroModel.SelectStartRandomHeroes();

        public void SetActive() => _selectWindowHeroView.SetActiveView();

        public void SetInactive() => _selectWindowHeroView.SetInactiveView();

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
        
        public void SetupRandomHeroes()
        {
            for (int i = 0; i < _selectWindowHeroModel._heroesForChose.Count; i++)
            {
                var data = _selectWindowHeroModel._heroesForChose[i];
                _selectWindowHeroView.heroViews[i].SetViewData(data);
            }
        }
    }
}