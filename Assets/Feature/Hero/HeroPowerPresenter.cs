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
        private HeroPowerView _playerHeroPowerView;
        private HeroPowerView _enemyHeroPowerView;

        public HeroPowerPresenter(HeroPowerSystem heroPowerSystem, HandViewSwitcher handViewSwitcher, GameSessionModel gameSessionModel)
        {
            _heroPowerSystem = heroPowerSystem;
            _handViewSwitcher = handViewSwitcher;
            _gameSessionModel = gameSessionModel;

            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        public void InitPlayer(HeroPowerView heroPowerView)
        {
            _playerHeroPowerView = heroPowerView;
            _heroPowerSystem.OnHeroPowerUsed += () =>
                UpdateHeroPowerView(_playerHeroPowerView, _gameSessionModel.PlayerHero);
            UpdateHeroPowerView(_playerHeroPowerView, _gameSessionModel.PlayerHero);
        }

        public void InitEnemy(HeroPowerView heroPowerView)
        {
            _enemyHeroPowerView = heroPowerView;
            _heroPowerSystem.OnEnemyHeroPowerUsed += () =>
            {
                UpdateHeroPowerView(_enemyHeroPowerView, _gameSessionModel.EnemyHero);
            };
               
            UpdateHeroPowerView(_enemyHeroPowerView, _gameSessionModel.EnemyHero);
        }

        private void UpdateHeroPowerView(HeroPowerView view, GameSessionPlayerData playerData)
        {
            if (view == null || playerData.CurrentHeroPower == null) return;
            bool canCast = !playerData.HeroPowerUsedThisTurn &&
                           playerData.MainHeroEntity().Chakra >= playerData.CurrentHeroPower.Cost;
            view.SetCanCastView(canCast);
            view.SetUsedThisTurnView(playerData.HeroPowerUsedThisTurn);
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            if (owner == _gameSessionModel.PlayerHero.MainHeroEntity())
                UpdateHeroPowerView(_playerHeroPowerView, _gameSessionModel.PlayerHero);
        }

        public void UpdateCanCastView()
        {
            _heroPowerSystem.UpdateBehaviour();
            UpdateHeroPowerView(_playerHeroPowerView, _gameSessionModel.PlayerHero);
        }
    }
}