using Feature.Entity.Script;
using Feature.GameSessionData;

namespace Feature.ShopGamePlay.Script.Currency
{
    public class CurrencyManagerSystem
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly CurrencyManagePresenter _currencyManagePresenter;
        private readonly EntityDeathSystem _entityDeathSystem;

        public CurrencyManagerSystem(GameSessionModel gameSessionModel, 
            CurrencyManagePresenter currencyManagePresenter,
            EntityDeathSystem entityDeathSystem)
        {
            _gameSessionModel = gameSessionModel;
            _currencyManagePresenter = currencyManagePresenter;
            _entityDeathSystem = entityDeathSystem;
            
            _entityDeathSystem.OnEntityDied += OnEntityKilled;
        }

        public void Init()
        {
            _currencyManagePresenter.SubscribeToCurrencyChanges();
        }

        public void NewTurnUpdate()
        {
            AddCurrency(_gameSessionModel.PlayerHero, 15);
            AddCurrency(_gameSessionModel.EnemyHero, 15);
        }

        private void OnEntityKilled(CardAndHealthEntityOwnerData victim, CardAndHealthEntityOwnerData killer)
        {
            if (killer == null) return;
    
            var killerPlayer = GetPlayerDataForOwner(killer);
            var victimPlayer = GetPlayerDataForOwner(victim);
    
            if (killerPlayer == null || victimPlayer == null) return;
    
            if (killerPlayer == victimPlayer) return;
    
            AddCurrency(killerPlayer, victim.Cost);
        }

        private void AddCurrency(GameSessionPlayerData playerData, int amount)
        {
            if (playerData == null || amount <= 0) return;
            
            playerData.Currency += amount;
        }

        private GameSessionPlayerData GetPlayerDataForOwner(CardAndHealthEntityOwnerData owner)
        {
            if (_gameSessionModel.PlayerHero.CardAndHealthEntityOwners.Contains(owner))
                return _gameSessionModel.PlayerHero;
            
            if (_gameSessionModel.EnemyHero.CardAndHealthEntityOwners.Contains(owner))
                return _gameSessionModel.EnemyHero;
            
            return null;
        }
    }
}