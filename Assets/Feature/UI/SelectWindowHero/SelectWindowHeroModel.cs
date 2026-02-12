using System.Collections.Generic;
using Feature.GameSessionData;

namespace Feature.UI.SelectWindowHero
{
    public class SelectWindowHeroModel
    {
        public GameSessionPlayerData _selectedHero;
        
        public List<GameSessionPlayerData> _heroesForChose;

        public int countPersonForChose = 5;

    }
}