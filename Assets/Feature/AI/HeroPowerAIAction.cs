using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.AI
{
    public class HeroPowerAIAction : IAIAction
    {
        private readonly SpellCardData _heroPower;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CardCastService _cardCastService;
        private readonly GameSessionModel _gameSessionModel;
    
        public int Cost => _heroPower.Cost;
        public TargetType TargetType => _heroPower.TargetType;

        public HeroPowerAIAction(SpellCardData heroPower, CardAndHealthEntityOwnerData owner, CardCastService cardCastService, GameSessionModel gameSessionModel)
        {
            _heroPower = heroPower;
            _owner = owner;
            _cardCastService = cardCastService;
            _gameSessionModel = gameSessionModel;
        }
        
        public void Execute(CardAndHealthEntityOwnerData target)
        {
            Debug.Log($"[AI] HeroPower: {_heroPower.Name} | Owner: {_owner._heroName} | Target: {target?._heroName ?? "none"} | Cost: {_heroPower.Cost}");
            _gameSessionModel.EnemyHero.HeroPowerUsedThisTurn = true; 
            _cardCastService.Cast(_heroPower, _owner, target);
        }
    }
}