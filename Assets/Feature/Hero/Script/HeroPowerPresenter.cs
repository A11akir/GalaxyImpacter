using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.HandLogic;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Hero.Script
{
    public class HeroPowerPresenter
    {
        private readonly HeroPowerSystem _heroPowerSystem;
        private readonly HandViewSwitcher _handViewSwitcher;
        private readonly GameSessionModel _gameSessionModel;

        private readonly List<HeroPowerGameplayView> _playerViews = new();
        private readonly List<HeroPowerGameplayView> _enemyViews = new();

        private readonly Dictionary<SpellCardData, HeroPowerGameplayView> _playerCardToView = new();
        private readonly Dictionary<SpellCardData, HeroPowerGameplayView> _enemyCardToView = new();

        private readonly Dictionary<PassiveEffectBase, HeroPowerGameplayView> _passiveToView = new();
        private readonly Dictionary<PassiveEffectBase, IDisposable> _valueSubscriptions = new();

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

                var heroPower = _gameSessionModel.PlayerHero.HeroPowers[i];
                _playerCardToView[heroPower] = view;

                _heroPowerSystem.OnHeroPowerUsed += () => UpdateHeroPowerView(view, _gameSessionModel.PlayerHero, index);
                UpdateHeroPowerView(view, _gameSessionModel.PlayerHero, index);
            }
        }

        public void InitEnemy(List<HeroPowerGameplayView> views)
        {
            var enemyHeroPowers = _gameSessionModel.EnemyHero.HeroPowers;

            for (int i = 0; i < views.Count; i++)
            {
                var view = views[i];
                _enemyViews.Add(view);

                if (i < enemyHeroPowers.Count)
                {
                    var heroPower = enemyHeroPowers[i];
                    _enemyCardToView[heroPower] = view;
                }

                _heroPowerSystem.OnEnemyHeroPowerUsed += () =>
                    UpdateHeroPowerView(view, _gameSessionModel.EnemyHero, 0);

                UpdateHeroPowerView(view, _gameSessionModel.EnemyHero, 0);
            }
        }

        public void HandlePassiveAdded(PassiveEffectBase passive, SpellCardData sourceCard, CardAndHealthEntityOwnerData owner)
        {
            bool isPlayer = owner == _gameSessionModel.PlayerHero.MainHeroEntity();
            var dict = isPlayer ? _playerCardToView : _enemyCardToView;

            if (!dict.TryGetValue(sourceCard, out var view)) return;

            _passiveToView[passive] = view;

            if (passive is IValueProvider valueProvider)
            {
                var sub = valueProvider.Value.Subscribe(_ => view.SetPassiveEffectData(passive));
                _valueSubscriptions[passive] = sub;
                view.SetPassiveEffectData(passive);
            }
            else
            {
                view.SetPassiveEffectData(passive);
            }
        }

        public void HandlePassiveRemoved(PassiveEffectBase passive)
        {
            if (_passiveToView.TryGetValue(passive, out var view))
            {
                view.ClearPassiveEffectData();
                _passiveToView.Remove(passive);
            }

            if (_valueSubscriptions.TryGetValue(passive, out var sub))
            {
                sub.Dispose();
                _valueSubscriptions.Remove(passive);
            }
        }

        private void UpdateHeroPowerView(HeroPowerGameplayView view, GameSessionPlayerData playerData, int index)
        {
            if (!view || playerData.HeroPowers == null || index >= playerData.HeroPowers.Count) return;

            if (playerData.HeroPowers[index].IsPassive)
            {
                view.SetPassiveView();
                return;
            }

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