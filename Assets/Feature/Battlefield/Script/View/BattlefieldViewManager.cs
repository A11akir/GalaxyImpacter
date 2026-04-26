
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Battlefield.Script.View
{
    public class BattlefieldViewManager
    {
        private readonly Dictionary<GameSessionPlayerData, List<CardOnBattlefieldView>> _battlefieldViews = new();
        private readonly Dictionary<CardAndHealthEntityOwnerData, CardOnBattlefieldView> _ownerToView = new();
        private readonly CardOnBattlefieldPresenter _presenter;

        public BattlefieldViewManager(CardOnBattlefieldPresenter presenter)
        {
            _presenter = presenter;
        }

        public void InitializeViews(GameSessionPlayerData playerData, GameObject battlefield)
        {
            _battlefieldViews[playerData] = GetCardViewsFromBattlefield(battlefield);
        }

        public CardOnBattlefieldView SetupView(MinionCardData card, int index, GameSessionPlayerData playerData)
        {
            var view = _battlefieldViews[playerData][index];
            _presenter.SetCardInBattlefield(view, card);
            return view;
        }

        public void RegisterOwnerView(CardAndHealthEntityOwnerData owner, CardOnBattlefieldView view)
        {
            _ownerToView[owner] = view;
        }

        public void UnregisterOwnerView(CardAndHealthEntityOwnerData owner)
        {
            if (_ownerToView.TryGetValue(owner, out var view))
            {
                view.ClearData();
                _ownerToView.Remove(owner);
            }
        }

        public CardOnBattlefieldView GetView(CardAndHealthEntityOwnerData owner)
        {
            return _ownerToView.TryGetValue(owner, out var view) ? view : null;
        }

        public void SetSelected(CardAndHealthEntityOwnerData owner, bool selected)
        {
            foreach (var kvp in _ownerToView)
                kvp.Value.SetSelected(kvp.Key == owner);
        }

        public int GetViewIndex(CardOnBattlefieldView view, GameSessionPlayerData playerData)
        {
            return _battlefieldViews[playerData].IndexOf(view);
        }

        private List<CardOnBattlefieldView> GetCardViewsFromBattlefield(GameObject battlefield)
        {
            var cardViews = new List<CardOnBattlefieldView>();
            foreach (Transform child in battlefield.transform)
            {
                var cardView = child.GetComponent<CardOnBattlefieldView>();
                cardViews.Add(cardView);
            }
            return cardViews;
        }
    }
}