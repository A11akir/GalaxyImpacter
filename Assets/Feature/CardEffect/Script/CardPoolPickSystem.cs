using Feature.Data;

namespace Feature.CardEffect.Script
{
    public class CardPoolPickSystem
    {
        private readonly GameData _gameData;

        public CardPoolPickSystem(GameData gameData) => _gameData = gameData;

        public CardStatsData Pick(CardPickQuery query, EffectContext ctx)
        {
            return null;
        }
    }
}