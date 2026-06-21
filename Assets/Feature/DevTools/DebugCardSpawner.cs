using System.Linq;
using Feature.Data;
using Feature.GameSessionData;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Feature.DevTools
{
    public class DebugCardSpawner : MonoBehaviour
    {
        [SerializeField] private GameData _gameData;
        [Inject] private GameSessionModel _gameSessionModel;

        [Title("Добавить карту в руку")]
        [SerializeField] private string _cardName;

        public enum TargetSide { Player, Enemy }

        [SerializeField] private TargetSide _target;

        [Button("Добавить карту")]
        private void AddCardToHand()
        {
            var card = _gameData.allCards
                .FirstOrDefault(c => c.Name == _cardName);

            if (card == null)
            { 
                Debug.LogWarning($"[DebugCardSpawner] Card '{_cardName}' not found in GameData.allCards");
                return;
            }

            var owner = _target == TargetSide.Player
                ? _gameSessionModel.PlayerHero.MainHeroEntity()
                : _gameSessionModel.EnemyHero.MainHeroEntity();

            var cardCopy = ScriptableObject.Instantiate(card);
            cardCopy.id = System.Guid.NewGuid().ToString();

            owner.AddCardToHand(cardCopy, owner.CountCardsInHand);

            Debug.Log($"[DebugCardSpawner] Added '{card.Name}' to {_target} hand");
        }
    }
}