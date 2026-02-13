using UnityEngine;

namespace Feature.GameSessionData
{
    public class GameSessionData
    {
        public int Turn;

        public GameSessionPlayerData PlayerHero;
        public GameSessionPlayerData EnemyHero;
        
        public GameSessionData(GameSessionPlayerData playerHero, GameSessionPlayerData enemyHero)
        {
            PlayerHero = playerHero;
            EnemyHero = enemyHero;
        }

        public void ChooseFirstPlayer()
        {
            bool playerFirst = new System.Random().Next(2) == 0;
            
            if (playerFirst)
            {
                PlayerHero.IsPlayerFirst = true;
                EnemyHero.IsPlayerFirst = false;
            }
            else
            {
                PlayerHero.IsPlayerFirst = false;
                EnemyHero.IsPlayerFirst = true;
            }
        }

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