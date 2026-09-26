using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.Hero.Script;

namespace Feature.AI
{
    public class HeroPowerAIAction : IAIAction
    {
        private readonly SpellCardData _heroPower;
        private readonly int _index; // ← добавили
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CardCastService _cardCastService;
        private readonly GameSessionModel _gameSessionModel;
        private readonly HeroPowerSystem _heroPowerSystem;
    
        public bool DealsDamage() => _heroPower.DealsDamage(); 
        public int Cost => _heroPower.Cost;
        public TargetType TargetType => _heroPower.TargetType;

        public HeroPowerAIAction(SpellCardData heroPower, int index, CardAndHealthEntityOwnerData owner, CardCastService cardCastService, GameSessionModel gameSessionModel, HeroPowerSystem heroPowerSystem)
        {
            _heroPower = heroPower;
            _index = index;
            _owner = owner;
            _cardCastService = cardCastService;
            _gameSessionModel = gameSessionModel;
            _heroPowerSystem = heroPowerSystem;
        }
    
        public void Execute(CardAndHealthEntityOwnerData target)
        {
            _gameSessionModel.EnemyHero.HeroPowerUsage.SetUsed(_index);
            _heroPowerSystem.NotifyEnemyHeroPowerUsed();
            _cardCastService.Cast(_heroPower, _owner, target);
        }
    }
}