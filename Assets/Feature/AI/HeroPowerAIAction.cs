using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using Feature.Hero;
using UnityEngine;

namespace Feature.AI
{
    public class HeroPowerAIAction : IAIAction
    {
        private readonly SpellCardData _heroPower;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CardCastService _cardCastService;
        private readonly GameSessionModel _gameSessionModel;
        private readonly HeroPowerSystem _heroPowerSystem;
        public bool DealsDamage() => _heroPower.DealsDamage(); 
        public int Cost => _heroPower.Cost;
        public TargetType TargetType => _heroPower.TargetType;

        public HeroPowerAIAction(SpellCardData heroPower, CardAndHealthEntityOwnerData owner, CardCastService cardCastService, GameSessionModel gameSessionModel, HeroPowerSystem heroPowerSystem)
        {
            _heroPower = heroPower;
            _owner = owner;
            _cardCastService = cardCastService;
            _gameSessionModel = gameSessionModel;
            _heroPowerSystem = heroPowerSystem;
        }
        
        public void Execute(CardAndHealthEntityOwnerData target)
        {
            _gameSessionModel.EnemyHero.HeroPowerUsage.SetUsed(0); // ← индекс 0 для врага
            _heroPowerSystem.NotifyEnemyHeroPowerUsed();
            _cardCastService.Cast(_heroPower, _owner, target);
        }
    }
}