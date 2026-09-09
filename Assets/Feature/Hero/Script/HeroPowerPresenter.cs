using System;
using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.HandLogic;
using Feature.PassiveEffect.Script;
using R3;
using UnityEngine;

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

        //Принимает все внутриконтейнерные вьюхи, стартовое количество абилок
        public void InitPlayer(List<HeroPowerGameplayView> views, int count)
        {
            //Цикл срабатывает столько раз сколько абилок было на старте
            for (int i = 0; i < views.Count && i < count; i++)
            {
                var index = i;
                var view = views[i];
                //нахуято отдельная вьюха для каждого героя заполняется
                _playerViews.Add(view);

                //дата карты
                var heroPower = _gameSessionModel.PlayerHero.HeroPowers[i];
                
                //словарь дата+вьюха
                _playerCardToView[heroPower] = view;

                //привязка для реакции на использование передается отдельно вьюха герой и индекс хотя как будто уже можно передавать _playerCardToView
                _heroPowerSystem.OnHeroPowerUsed += () => UpdateHeroPowerView(view, _gameSessionModel.PlayerHero, index);
                UpdateHeroPowerView(view, _gameSessionModel.PlayerHero, index);
            }
            
            for (int i = count; i < views.Count; i++)
            {
                _playerViews.Add(views[i]); // ← добавляем в список
                views[i].gameObject.SetActive(false);
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

            // скрываем слоты после количества сил героя 
            for (int i = enemyHeroPowers.Count; i < views.Count; i++)
            {
                _enemyViews.Add(views[i]);
                views[i].gameObject.SetActive(false);
            }
               
        }

        private HeroPowerGameplayView GetFreePassiveSlot(CardAndHealthEntityOwnerData owner)
        {
            var views = owner == _gameSessionModel.PlayerHero.MainHeroEntity()
                ? _playerViews
                : _enemyViews;

            var cardToView = owner == _gameSessionModel.PlayerHero.MainHeroEntity()
                ? _playerCardToView
                : _enemyCardToView;

            foreach (var view in views)
            {
                bool occupiedByPassive = _passiveToView.ContainsValue(view);
                bool occupiedByCard = cardToView.ContainsValue(view);

                if (!occupiedByPassive && !occupiedByCard)
                {
                    return view;
                }
            }
            
            return null;
        }
        
        public void HandlePassiveAdded(PassiveEffectBase passive, CardAndHealthEntityOwnerData owner)
        {
            var slot = GetFreePassiveSlot(owner); // ← передаём owner
            if (slot == null) return;

            slot.gameObject.SetActive(true);
            slot.SetDataView(passive.SourceCard);
            _passiveToView[passive] = slot;

            if (passive is IValueProvider valueProvider)
            {
                var sub = valueProvider.Value.Subscribe(_ => slot.SetPassiveEffectData(passive));
                _valueSubscriptions[passive] = sub;
            }

            slot.SetPassiveEffectData(passive);
        }

        public void HandlePassiveRemoved(PassiveEffectBase passive)
        {
            if (_passiveToView.TryGetValue(passive, out var slot))
            {
                slot.ClearPassiveEffectData();
                slot.gameObject.SetActive(false);
                _passiveToView.Remove(passive);
            }

            if (_valueSubscriptions.TryGetValue(passive, out var sub))
            {
                sub.Dispose();
                _valueSubscriptions.Remove(passive);
            }
        }
        //пока что обработка устаноавленной абилки а не полнцоенного слота, реакция на использование опять же можно обойтись только _playerCardToView в параметрах
        private void UpdateHeroPowerView(HeroPowerGameplayView view, GameSessionPlayerData playerData, int index)
        {
            // если данные сущетсвует то запускаем
            if (!view || playerData.HeroPowers == null || index >= playerData.HeroPowers.Count) return;

            //если пасивка то не нужно реакция на трату маны или уже использование
            if (playerData.HeroPowers[index].IsPassive)
            {
                //убирает крисатл маны и окно использования
                view.SetPassiveView();
                return;
            }

            //можео ли сыграть, хватает ли маны или играл ли до этого
            bool canCast = !playerData.HeroPowerUsage.IsUsed(index) &&
                           playerData.MainHeroEntity().Chakra >= playerData.HeroPowers[index].Cost;

            view.SetCanCastView(canCast);
            //сюда можно передавать canCast тоже вроде надо потестить
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