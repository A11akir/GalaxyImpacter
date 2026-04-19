
using System.Collections.Generic;
using System.Linq;

namespace Feature.GameSessionData
{
    public class GameSessionModel
    {
        public int Turn = 0;
        
        public GameSessionPlayerData PlayerHero;
        public GameSessionPlayerData EnemyHero;
        public GameSessionPlayerData GetPlayerDataByOwner(CardAndHealthEntityOwnerData owner)
        {
            if (PlayerHero.CardAndHealthEntityOwners.Contains(owner)) return PlayerHero;
            if (EnemyHero.CardAndHealthEntityOwners.Contains(owner)) return EnemyHero;
            return null;
        }
        
        public IEnumerable<CardAndHealthEntityOwnerData> GetAllEntityOwners()
        {
            return PlayerHero.CardAndHealthEntityOwners
                .Concat(EnemyHero.CardAndHealthEntityOwners);
        }
        public GameSessionModel(GameSessionPlayerData playerHero, GameSessionPlayerData enemyHero)
        {
            PlayerHero = playerHero;
            EnemyHero = enemyHero;
        }
        
        public float PrepareStartTime = 10f;
        public float FightStartTime = 5f;
        
        public bool IsFirstTurn() => Turn == 1;

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

        public bool PlayerStartGameSessionFirst() => PlayerHero.IsPlayerFirst;

        
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