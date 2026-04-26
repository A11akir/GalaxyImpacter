
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
        
        public IEnumerable<CardAndHealthEntityOwnerData> GetEnemyOwnersFor(CardAndHealthEntityOwnerData owner)
        {
            bool ownerIsPlayer = PlayerHero.GetAllEntityOwners().Contains(owner);
            var enemyPlayerData = ownerIsPlayer ? EnemyHero : PlayerHero;
    
            yield return enemyPlayerData.MainHeroEntity();
            foreach (var entity in enemyPlayerData.CardAndHealthEntityOwners)
                yield return entity;
        }
        
        public bool AreAllies(CardAndHealthEntityOwnerData a, CardAndHealthEntityOwnerData b)
        {
            var playerOwners = PlayerHero.GetAllEntityOwners();
            bool bothArePlayerSide = playerOwners.Contains(a) && playerOwners.Contains(b);
            bool bothAreEnemySide = !playerOwners.Contains(a) && !playerOwners.Contains(b);
            return bothArePlayerSide || bothAreEnemySide;
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