using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;

namespace Feature.AI
{
    public class CardAIAction : IAIAction
    {
        private readonly CardStatsData _card;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CombatSystem.CombatSystem _combatSystem;
        private readonly BattlefieldSystem _battlefieldSystem;
    
        public int Cost => _card.Cost;
        public TargetType TargetType => _card.TargetType;

        public CardAIAction(CardStatsData card, CardAndHealthEntityOwnerData owner, 
            BattlefieldSystem battlefieldSystem, CombatSystem.CombatSystem combatSystem)
        {
            _card = card;
            _owner = owner;
            _battlefieldSystem = battlefieldSystem;
            _combatSystem = combatSystem;
        }

        public void Execute(CardAndHealthEntityOwnerData target, GameSessionModel gameSessionModel)
        {
            var handCardData = new HandCardData(data: _card, view: null, behaviour: null, logic: null);
            var logic = new GameplayLogicCard(handCardData, gameSessionModel, _battlefieldSystem, _combatSystem);
            logic.CastCard(_owner, target);
        }
    }
}