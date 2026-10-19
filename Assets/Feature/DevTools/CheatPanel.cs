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

        [Title("Добавить ману")]
        [SerializeField] private int _manaAmount = 2;

        [Button("Добавить карту")]
        private void AddCardToHand()
        {
            var card = _gameData.allCards
                .FirstOrDefault(c => string.Equals(c.Name, _cardName, System.StringComparison.OrdinalIgnoreCase));

            if (card == null)
            {
                Debug.LogWarning($"[DebugCardSpawner] Card '{_cardName}' not found in GameData.allCards");
                return;
            }

            var owner = _target switch
            {
                TargetSide.Player => _gameSessionModel.PlayerHero.MainHeroEntity(),
                TargetSide.Enemy => _gameSessionModel.EnemyHero.MainHeroEntity(),
                TargetSide.CurrentActiveOwner => _handViewSwitcher.CurrentOwner,
                _ => null
            };

            if (owner == null)
            {
                Debug.LogWarning("[DebugCardSpawner] No active owner found");
                return;
            }

            var cardCopy = ScriptableObject.Instantiate(card);
            cardCopy.id = System.Guid.NewGuid().ToString();

            owner.AddCardToHand(cardCopy, owner.CountCardsInHand);

        }

        [Button("Добавить ману")]
        private void AddMana()
        {
            var owner = _target switch
            {
                TargetSide.Player => _gameSessionModel.PlayerHero.MainHeroEntity(),
                TargetSide.Enemy => _gameSessionModel.EnemyHero.MainHeroEntity(),
                TargetSide.CurrentActiveOwner => _handViewSwitcher.CurrentOwner,
                _ => null
            };

            if (owner == null)
            {
                Debug.LogWarning("[CheatPanel] No active owner found");
                return;
            }

            int max = owner.MaxChakraCountBaseIncrease;
            owner.Chakra = Mathf.Min(owner.Chakra + _manaAmount, max);
        }

        public enum TargetSide { Player, Enemy, CurrentActiveOwner }
    }
}