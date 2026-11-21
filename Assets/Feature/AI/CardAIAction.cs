using System.Linq;
using Feature.AI;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;

public class CardAIAction : IAIAction
{
    private readonly CardStatsData _card;
    private readonly CardAndHealthEntityOwnerData _owner;
    private readonly CardCastService _cardCastService;
    
    public int Cost => _card.Cost;
    public TargetType TargetType => _card.TargetType;

    public bool DealsDamage() =>
        _card is SpellCardData spell && spell.Effects.Any(e => e is DamageEffect);

    public CardAIAction(CardStatsData card, CardAndHealthEntityOwnerData owner, CardCastService cardCastService)
    {
        _card = card;
        _owner = owner;
        _cardCastService = cardCastService;
    }
    
    public void Execute(CardAndHealthEntityOwnerData target)
    {
        _cardCastService.Cast(_card, _owner, target);
    }
}
