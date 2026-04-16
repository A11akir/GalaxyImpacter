using Feature.Battlefield.Script.View;
using Feature.Card.Script;
using Feature.GoogleSheets;

namespace Feature.Battlefield.Script
{
    public class CardOnBattlefieldPresenter
    {
        public void SetCardInBattlefield(CardOnBattlefieldView view, MinionCardData cardStatsData) => 
            view.SetDataView(cardStatsData);
    }
}