using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;

namespace Feature.AI
{
    public class HeroPowerAIAction : IAIAction
    {
        private readonly SpellCardData _heroPower;
        private readonly CardAndHealthEntityOwnerData _owner;
    
        public int Cost => _heroPower.Cost;
        public TargetType TargetType => _heroPower.TargetType;

        public HeroPowerAIAction(SpellCardData heroPower, CardAndHealthEntityOwnerData owner)
        {
            _heroPower = heroPower;
            _owner = owner;
        }

        public void Execute(CardAndHealthEntityOwnerData target)
        {
            // логика каста хироповера
        }
    }
}