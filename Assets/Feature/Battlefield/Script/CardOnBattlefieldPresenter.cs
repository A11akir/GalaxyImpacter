using Feature.Card.Script;

namespace Feature.Battlefield.Script
{
    public class CardOnBattlefieldPresenter
    {
        public void SetCardInPlayerHand(CardOnBattlefieldView view, CardStatsData cardStatsData)
        {
            view.SetDataView(cardStatsData);
        }
    }
}