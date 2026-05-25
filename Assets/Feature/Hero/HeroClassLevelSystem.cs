using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.Hero
{
    public class HeroClassLevelSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly CompositeDisposable _disposables = new();

        public HeroClassLevelSystem(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }

        public void Init()
        {
            _gameSessionModel.PlayerHero.MainHeroEntity().CardsInDeck
                .Subscribe(_ =>
                {
                    Debug.Log("[HeroClassLevelSystem] CardsInDeck changed, recalculating...");
                    var baseDeck = _gameSessionModel.PlayerHero.MainHeroEntity().BaseDeck;
                    Debug.Log($"[HeroClassLevelSystem] BaseDeck count: {baseDeck.Count}");
                    _gameSessionModel.PlayerHero.HeroClassLevel.RecalculateFromDeck(baseDeck);
                })
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}