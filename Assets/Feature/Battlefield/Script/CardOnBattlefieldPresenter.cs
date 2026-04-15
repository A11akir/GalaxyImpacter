using Feature.Battlefield.Script.View;
using Feature.Card.Script;

namespace Feature.Battlefield.Script
{
    public class CardOnBattlefieldPresenter
    {
        public void SetCardInPlayerHand(CardOnBattlefieldView view, MinionCardData cardStatsData) => 
            view.SetDataView(cardStatsData);
    }
}