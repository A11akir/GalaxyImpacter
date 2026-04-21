using System;
using Feature.Battlefield.Script;
using Feature.Card.Script;
using Feature.GameSessionData;
using UnityEngine;


namespace Feature.AI
{
    public class CardAIAction : IAIAction
    {
        private readonly CardStatsData _card;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CardCastService _cardCastService;
        public int Cost => _card.Cost;
        public TargetType TargetType => _card.TargetType;

        public CardAIAction(CardStatsData card, CardAndHealthEntityOwnerData owner, CardCastService cardCastService)
        {
            _card = card;
            _owner = owner;
            _cardCastService = cardCastService;
        }

        
        public void Execute(CardAndHealthEntityOwnerData target)
        {
            Debug.Log($"[AI] Cast: {_card.Name} | Owner: {_owner._heroName} | Target: {target?._heroName ?? "none"} | Cost: {_card.Cost} | Chakra before: {_owner.Chakra}");
            _cardCastService.Cast(_card, _owner, target);
            Debug.Log($"[AI] After cast | Chakra left: {_owner.Chakra}");
        }
    }
}