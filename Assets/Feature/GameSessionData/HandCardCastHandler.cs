using Feature.Card.Script;

namespace Feature.GameSessionData
{
    public class HandCardCastHandler
    {
        private readonly HandCardData _cardData;
        private readonly CardCastService _cardCastService;

        public HandCardCastHandler(HandCardData cardData, CardCastService cardCastService)
        {
            _cardData = cardData;
            _cardCastService = cardCastService;
        }

        public void CastCard(CardAndHealthEntityOwnerData owner, CardAndHealthEntityOwnerData target)
        {
            _cardCastService.Cast(_cardData.Data, owner, target);
        }
    }
}