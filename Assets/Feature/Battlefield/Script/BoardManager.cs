// 1. BoardManager — объединяет state tracking и slot management

using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script.View;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using R3;

namespace Feature.Battlefield.Script
{
    public class BoardManager
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly TipPlaceBattlefieldViewSystem _tipPlaceSystem;
        private readonly Dictionary<GameSessionPlayerData, List<MinionCardData>> _previousCards = new();

        public event Action<MinionCardData, int, GameSessionPlayerData> OnCardAdded;
        public event Action<MinionCardData, GameSessionPlayerData> OnCardRemoved;

        public BoardManager(GameSessionModel gameSessionModel, TipPlaceBattlefieldViewSystem tipPlaceSystem)
        {
            _gameSessionModel = gameSessionModel;
            _tipPlaceSystem = tipPlaceSystem;
        }

        public List<CardAndHealthEntityOwnerData> GetEntitySlotsForTargets(GameSessionPlayerData playerData)
        {
            bool isEnemy = _gameSessionModel.EnemyHero == playerData;
            var result = new List<CardAndHealthEntityOwnerData>();

            
            //для противника герой добавляется до цикла, для героя после
            if (isEnemy)
                result.Add(playerData.MainHeroEntity());

            foreach (var card in playerData.CardsInBoardList)
            {
                if (card == null) continue;

                var owner = playerData.CardAndHealthEntityOwners
                                .FirstOrDefault(o => o.CardId == card.id)
                            ?? throw new InvalidOperationException($"Нет сущности для карты {card.id} в слоте");

                result.Add(owner);
            }

            if (!isEnemy)
                result.Add(playerData.MainHeroEntity());

            return result;
        }

        public void Subscribe(GameSessionPlayerData playerData)
        {
            _previousCards[playerData] = new();

            playerData.CardsInBoard.Subscribe(currentCards =>
            {
                var nonNullCards = currentCards.Where(c => c != null).ToList();
                var previousNonNullCards = _previousCards[playerData].Where(c => c != null).ToList();

                var addedCards = nonNullCards
                    .Where(c => previousNonNullCards.All(p => p.id != c.id))
                    .ToList();

                var removedCards = previousNonNullCards
                    .Where(p => nonNullCards.All(c => c.id != p.id))
                    .ToList();

                if (removedCards.Count > 0)
                    OnCardRemoved?.Invoke(removedCards[0], playerData);

                if (addedCards.Count > 0)
                {
                    int addedIndex = currentCards.FindIndex(c => c != null && c.id == addedCards[0].id);
                    OnCardAdded?.Invoke(addedCards[0], addedIndex, playerData);
                }

                _previousCards[playerData] = currentCards.ToList();
            });
        }

        // Slot management
        public int GetFreeSlotForPlayer()
        {
            return _tipPlaceSystem.GetCardIndex();
        }

        public int GetRandomFreeSlotForEnemy()
        {
            var enemyBoard = _gameSessionModel.EnemyHero.CardsInBoardList;
            var freeSlots = new List<int>();

            for (int i = 0; i < enemyBoard.Count; i++)
                if (enemyBoard[i] == null)
                    freeSlots.Add(i);

            return freeSlots.Count == 0 ? -1 : freeSlots[UnityEngine.Random.Range(0, freeSlots.Count)];
        }

        /// <summary>
        /// Ближайший свободный слот к center: сначала сам слот, потом вправо, потом влево.
        /// center &lt; 0 — ищем с первого слота.
        /// </summary>
        public int FindNearestFreeSlot(GameSessionPlayerData playerData, int center)
        {
            var board = playerData.CardsInBoardList;

            if (center < 0)
                center = 0;

            if (center < board.Count && board[center] == null)
                return center;

            for (int dist = 1; dist < board.Count; dist++)
            {
                int right = center + dist;
                if (right < board.Count && board[right] == null)
                    return right;

                int left = center - dist;
                if (left >= 0 && board[left] == null)
                    return left;
            }

            return -1;
        }

        public void OccupySlot(int index)
        {
            _tipPlaceSystem.OccupySlot(index);
        }

        public void FreeSlot(int index)
        {
            _tipPlaceSystem.FreeSlot(index);
        }
    }
}