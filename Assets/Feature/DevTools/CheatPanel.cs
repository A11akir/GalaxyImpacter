using System.Collections.Generic;
using System.Linq;
using Feature.Data;
using Feature.GameSessionData;
using Feature.HandLogic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Feature.DevTools
{
    public class CheatPanel : MonoBehaviour
    {
        [SerializeField] private GameData _gameData;
        [Inject] private GameSessionModel _gameSessionModel;
        [Inject] private HandViewSwitcher _handViewSwitcher;

        [Title("Добавить карту в руку")]
        [SerializeField] private string _cardName;
        [SerializeField] private TargetSide _target;

        [Title("Последние карты из чит-панели")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "Name")]
        [SerializeField] private List<CardStatsData> _recentCards = new();

        [Title("Добавить ману")]
        [SerializeField] private int _manaAmount = 2;

        [Button("$RecentName1"), GUIColor(0.8f, 1f, 0.8f), PropertySpace]
        private void AddRecent1() => AddRecentCard(0);

        [Button("$RecentName2"), GUIColor(0.8f, 1f, 0.8f)]
        private void AddRecent2() => AddRecentCard(1);

        [Button("$RecentName3"), GUIColor(0.8f, 1f, 0.8f)]
        private void AddRecent3() => AddRecentCard(2);

        private string RecentName1 => GetRecentName(0);
        private string RecentName2 => GetRecentName(1);
        private string RecentName3 => GetRecentName(2);

        private string GetRecentName(int index) =>
            index < _recentCards.Count && _recentCards[index] != null ? _recentCards[index].Name : "-";

#if UNITY_EDITOR
        private const string RecentCardsKey = "GalaxyImpacter.CheatPanel.RecentCards";

        private void OnEnable() => LoadRecentCards();
        private void OnDisable() => SaveRecentCards();

        private void LoadRecentCards()
        {
            _recentCards.Clear();

            var stored = UnityEditor.EditorPrefs.GetString(RecentCardsKey, string.Empty);
            if (string.IsNullOrEmpty(stored)) return;

            foreach (var name in stored.Split('|'))
            {
                var card = _gameData.allCards.FirstOrDefault(c => c.Name == name);
                if (card != null) _recentCards.Add(card);
                if (_recentCards.Count >= 3) break;
            }
        }

        private void SaveRecentCards()
        {
            var names = string.Join("|", _recentCards.Where(c => c != null).Select(c => c.Name));
            UnityEditor.EditorPrefs.SetString(RecentCardsKey, names);
        }
#endif

        private void AddRecentCard(int index)
        {
            if (index >= _recentCards.Count || _recentCards[index] == null) return;

            AddCardToHand(_recentCards[index]);
        }

        [Button("Добавить карту")]
        private void AddCardByName()
        {
            var card = _gameData.allCards
                .FirstOrDefault(c => string.Equals(c.Name, _cardName, System.StringComparison.OrdinalIgnoreCase));

            if (card == null)
            {
                Debug.LogWarning($"[DebugCardSpawner] Card '{_cardName}' not found in GameData.allCards");
                return;
            }

            AddCardToHand(card);
        }

        private void AddCardToHand(CardStatsData card)
        {
            var owner = GetTargetOwner();

            if (owner == null)
            {
                Debug.LogWarning("[DebugCardSpawner] No active owner found");
                return;
            }

            var cardCopy = ScriptableObject.Instantiate(card);
            cardCopy.id = System.Guid.NewGuid().ToString();

            owner.AddCardToHand(cardCopy, owner.CountCardsInHand);

            RememberCard(card);
        }

        // на кнопки попадают только карты, добавленные через чит-панель
        private void RememberCard(CardStatsData card)
        {
            _recentCards.RemoveAll(c => c == null || c.Name == card.Name);
            _recentCards.Insert(0, card);

            if (_recentCards.Count > 3)
                _recentCards.RemoveAt(_recentCards.Count - 1);

#if UNITY_EDITOR
            SaveRecentCards();
#endif
        }

        [Button("Добавить ману")]
        private void AddMana()
        {
            var owner = GetTargetOwner();

            if (owner == null)
            {
                Debug.LogWarning("[CheatPanel] No active owner found");
                return;
            }

            int max = owner.MaxChakraCountBaseIncrease;
            owner.Chakra = Mathf.Min(owner.Chakra + _manaAmount, max);
        }

        private CardAndHealthEntityOwnerData GetTargetOwner() =>
            _target switch
            {
                TargetSide.Player => _gameSessionModel.PlayerHero.MainHeroEntity(),
                TargetSide.Enemy => _gameSessionModel.EnemyHero.MainHeroEntity(),
                TargetSide.CurrentActiveOwner => _handViewSwitcher.CurrentOwner,
                _ => null
            };

        public enum TargetSide { Player, Enemy, CurrentActiveOwner }
    }
}
