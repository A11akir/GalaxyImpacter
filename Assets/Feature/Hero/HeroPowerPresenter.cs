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
        private HeroPowerGameplayView _playerHeroPowerGameplayView;
        private HeroPowerGameplayView _enemyHeroPowerGameplayView;

        public HeroPowerPresenter(HeroPowerSystem heroPowerSystem, HandViewSwitcher handViewSwitcher, GameSessionModel gameSessionModel)
        {
            _heroPowerSystem = heroPowerSystem;
            _handViewSwitcher = handViewSwitcher;
            _gameSessionModel = gameSessionModel;

            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        public void InitPlayer(HeroPowerGameplayView heroPowerGameplayView)
        {
            _playerHeroPowerGameplayView = heroPowerGameplayView;
            _heroPowerSystem.OnHeroPowerUsed += () =>
                UpdateHeroPowerView(_playerHeroPowerGameplayView, _gameSessionModel.PlayerHero);
            UpdateHeroPowerView(_playerHeroPowerGameplayView, _gameSessionModel.PlayerHero);
        }

        public void InitEnemy(HeroPowerGameplayView heroPowerGameplayView)
        {
            _enemyHeroPowerGameplayView = heroPowerGameplayView;
            _heroPowerSystem.OnEnemyHeroPowerUsed += () =>
            {
                UpdateHeroPowerView(_enemyHeroPowerGameplayView, _gameSessionModel.EnemyHero);
            };
               
            UpdateHeroPowerView(_enemyHeroPowerGameplayView, _gameSessionModel.EnemyHero);
        }

        private void UpdateHeroPowerView(HeroPowerGameplayView gameplayView, GameSessionPlayerData playerData)
        {
            if (!gameplayView || !playerData.CurrentHeroPower) return;
            bool canCast = !playerData.HeroPowerUsedThisTurn &&
                           playerData.MainHeroEntity().Chakra >= playerData.CurrentHeroPower.Cost;
            gameplayView.SetCanCastView(canCast);
            gameplayView.SetUsedThisTurnView(playerData.HeroPowerUsedThisTurn);
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            if (owner == _gameSessionModel.PlayerHero.MainHeroEntity())
                UpdateHeroPowerView(_playerHeroPowerGameplayView, _gameSessionModel.PlayerHero);
        }

        public void UpdateCanCastView()
        {
            _heroPowerSystem.UpdateBehaviour();
            UpdateHeroPowerView(_playerHeroPowerGameplayView, _gameSessionModel.PlayerHero);
        }
    }
}