using System;
using System.Linq;
using Feature.Card.Script;
using Feature.EndGameSession;
using Feature.GameSessionData;
using Feature.Hero;
using R3;

namespace Feature.Entity.Script
{
    public class EntityDeathSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();
        private readonly GameOverSystem _gameOverSystem;

        public event Action<CardAndHealthEntityOwnerData> OnEntityDied;

        public EntityDeathSystem(GameSessionModel gameSessionModel, HandDataRepository handDataRepository, GameOverSystem gameOverSystem)
        {
            _gameSessionModel = gameSessionModel;
            _handDataRepository = handDataRepository;
            _gameOverSystem = gameOverSystem;
        }

        public void Init(CardAndHealthEntityOwnerData owner)
        {
            owner.Health
                .Subscribe(hp =>
                {
                    if (hp <= 0)
                        HandleEntityDied(owner);
                })
                .AddTo(_disposables);
        }

        private void HandleEntityDied(CardAndHealthEntityOwnerData owner)
        {
            _handDataRepository.DisposeOwner(owner);

            var playerData = _gameSessionModel.GetPlayerDataByOwner(owner);
            if (playerData == null) return;

            if (playerData.MainHeroEntity() == owner)
            {
                bool isPlayer = playerData == _gameSessionModel.PlayerHero;
                _gameOverSystem.HandleGameOver(isPlayer);
                return;
            }

            var cardOnBoard = playerData.CardsInBoard.CurrentValue
                .FirstOrDefault(c => c != null && c.id == owner.CardId);

            if (cardOnBoard != null)
                playerData.RemoveCardFromBoard(cardOnBoard);

            OnEntityDied?.Invoke(owner);
        }

        public void Dispose() => _disposables.Dispose();
    }
}