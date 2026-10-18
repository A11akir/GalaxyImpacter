
using System;
using System.Linq;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.EndGameSession;
using Feature.GameSessionData;
using R3;
using UnityEngine;

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
                .Where(hp => hp <= 0)
                .Take(1)
                .Subscribe(_ => HandleEntityDied(owner))
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

            int victimSlot = playerData.CardsInBoardList.FindIndex(c => c != null && c.id == victim.CardId);

            OnEntityDied?.Invoke(victim, victim.LastDamageSource);
            
            if (cardOnBoard != null)
                playerData.RemoveCardFromBoard(cardOnBoard);

            TriggerDeathRattles(victim, victim.LastDamageSource, victimSlot);
        }

        private void TriggerDeathRattles(CardAndHealthEntityOwnerData victim, CardAndHealthEntityOwnerData killer, int victimSlot)
        {
            foreach (var passive in victim.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is DeathRattlePassive deathRattle)
                    deathRattle.Trigger(victim, killer, victimSlot);
        }

        public void Dispose() => _disposables.Dispose();
    }
}