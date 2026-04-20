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
        private HeroPowerView _heroPowerView;

        public HeroPowerPresenter(HeroPowerSystem heroPowerSystem, HandViewSwitcher handViewSwitcher, GameSessionModel gameSessionModel)
        {
            _heroPowerSystem = heroPowerSystem;
            _handViewSwitcher = handViewSwitcher;
            _gameSessionModel = gameSessionModel;

            _heroPowerSystem.OnHeroPowerUsed += OnHeroPowerUsed;
            _handViewSwitcher.OnOwnerSwitched += OnOwnerSwitched;
        }

        public void Init(HeroPowerView heroPowerView)
        {
            _heroPowerView = heroPowerView;
            UpdateCanCastView();
        }

        private void OnHeroPowerUsed()
        {
            _heroPowerView?.SetCanCastView(false);
        }

        private void OnOwnerSwitched(CardAndHealthEntityOwnerData owner)
        {
            if (owner == _gameSessionModel.PlayerHero.MainHeroEntity())
                _heroPowerView?.SetCanCastView(_heroPowerSystem.CanCast);
        }

        public void UpdateCanCastView()
        {
            _heroPowerSystem.UpdateBehaviour();
            _heroPowerView?.SetCanCastView(_heroPowerSystem.CanCast);
        }
    }
}