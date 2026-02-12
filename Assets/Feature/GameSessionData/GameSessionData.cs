namespace Feature.GameSessionData
{
    public class GameSessionData
    {
        public int Turn;

        public GameSessionPlayerData PlayerHero;
        public GameSessionPlayerData EnemyHero;

        public bool PlayersHaveHero()
        {
            if (PlayerHero.PlayerHasHero() &&
                EnemyHero.PlayerHasHero())
            {
                return true;
            }
            return false;
        }
        
        
    }
}