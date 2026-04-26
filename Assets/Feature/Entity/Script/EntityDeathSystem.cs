
using System;
using System.Linq;
using Feature.Card.Script;
using Feature.EndGameSession;
using Feature.GameSessionData;
using R3;

namespace Feature.Entity.Script
{
    public class EntityDeathSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly HandDataRepository _handDataRepository;
        private readonly CompositeDisposable _disposables = new();
        private readonly GameOverSystem _gameOverSystem;

        public event Action<CardAndHealthEntityOwnerData, CardAndHealthEntityOwnerData> OnEntityDied;

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

        private void HandleEntityDied(CardAndHealthEntityOwnerData victim)
        {
            _handDataRepository.DisposeOwner(victim);

            var playerData = _gameSessionModel.GetPlayerDataByOwner(victim);
            if (playerData == null) return;

            if (playerData.MainHeroEntity() == victim)
            {
                bool isPlayer = playerData == _gameSessionModel.PlayerHero;
                _gameOverSystem.HandleGameOver(isPlayer);
                return;
            }

            var cardOnBoard = playerData.CardsInBoard.CurrentValue
                .FirstOrDefault(c => c != null && c.id == victim.CardId);

            OnEntityDied?.Invoke(victim, victim.LastDamageSource);

            if (cardOnBoard != null)
                playerData.RemoveCardFromBoard(cardOnBoard);
        }

        public void Dispose() => _disposables.Dispose();
    }
}