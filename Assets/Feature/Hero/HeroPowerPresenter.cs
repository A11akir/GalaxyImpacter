using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.HandLogic;
using UnityEngine;

namespace Feature.Hero
{
    public class HeroPowerPresenter
    {
        private readonly HeroPowerSystem _heroPowerSystem;
        private readonly HandViewSwitcher _handViewSwitcher;
        private readonly GameSessionModel _gameSessionModel;
        
        private readonly List<HeroPowerGameplayView> _playerViews = new();
        private readonly List<HeroPowerGameplayView> _enemyViews = new();

        public HeroPowerPresenter(HeroPowerSystem heroPowerSystem, HandViewSwitcher handViewSwitcher, GameSessionModel gameSessionModel)
        {
            _heroPowerSystem = heroPowerSystem;
            _handViewSwitcher = handViewSwitcher;
            _gameSessionModel = gameSessionModel;

            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        public void InitPlayer(List<HeroPowerGameplayView> views, int count)
        {
            for (int i = 0; i < views.Count && i < count; i++)
            {
                var index = i;
                var view = views[i];
                _playerViews.Add(view);

                _heroPowerSystem.OnHeroPowerUsed += () =>
                    UpdateHeroPowerView(view, _gameSessionModel.PlayerHero, index);

                UpdateHeroPowerView(view, _gameSessionModel.PlayerHero, index);
            }
        }

        public void InitEnemy(List<HeroPowerGameplayView> views)
        {
            foreach (var view in views)
            {
                var capturedView = view;
                _enemyViews.Add(capturedView);

                _heroPowerSystem.OnEnemyHeroPowerUsed += () =>
                    UpdateHeroPowerView(capturedView, _gameSessionModel.EnemyHero, 0);

                UpdateHeroPowerView(capturedView, _gameSessionModel.EnemyHero, 0);
            }
        }

        private void UpdateHeroPowerView(HeroPowerGameplayView view, GameSessionPlayerData playerData, int index)
        {
            if (!view || playerData.HeroPowers == null || index >= playerData.HeroPowers.Count) return;

            bool canCast = !playerData.HeroPowerUsage.IsUsed(index) &&
                           playerData.MainHeroEntity().Chakra >= playerData.HeroPowers[index].Cost;

            view.SetCanCastView(canCast);
            view.SetUsedThisTurnView(playerData.HeroPowerUsage.IsUsed(index));
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            if (owner != _gameSessionModel.PlayerHero.MainHeroEntity()) return;

            for (int i = 0; i < _playerViews.Count; i++)
                UpdateHeroPowerView(_playerViews[i], _gameSessionModel.PlayerHero, i);
        }

        public void UpdateCanCastView()
        {
            _heroPowerSystem.UpdateBehaviour();

            for (int i = 0; i < _playerViews.Count; i++)
                UpdateHeroPowerView(_playerViews[i], _gameSessionModel.PlayerHero, i);
        }
    }
}