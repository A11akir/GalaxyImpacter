using System.Collections.Generic;
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
                    var baseDeck = _gameSessionModel.PlayerHero.MainHeroEntity().BaseDeck;
                    _gameSessionModel.PlayerHero.HeroClassLevel.RecalculateFromDeck(baseDeck);
                    RecalculateClassData(baseDeck);
                })
                .AddTo(_disposables);
        }
        
        private void RecalculateClassData(IEnumerable<CardStatsData> baseDeck)
        {
            var heroClassData = _gameSessionModel.PlayerHero.HeroClassData;

            foreach (var card in baseDeck)
            {
                if (card?.Specialization == null) continue;

                foreach (var spec in card.Specialization)
                {
                    if (!System.Enum.TryParse<AllHeroClass>(spec, out var heroClass)) continue;
                    if (heroClass == AllHeroClass.All) continue;

                    heroClassData.AddClass(heroClass);
                }
            }
        }

        public void Dispose() => _disposables.Dispose();
    }
}