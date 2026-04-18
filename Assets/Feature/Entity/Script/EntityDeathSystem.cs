using System.Linq;
using Feature.Battlefield.Script;
using Feature.GameSessionData;
using R3;

namespace Feature.Entity.Script
{
    public class EntityDeathSystem
    {
        private readonly CompositeDisposable _disposables = new();
        private GameSessionModel _gameSessionModel;
        private BattlefieldSystem _battlefieldSystem;

        public EntityDeathSystem(GameSessionModel gameSessionModel, BattlefieldSystem battlefieldSystem)
        {
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
        }


        public void Init(CardAndHealthEntityOwnerData owner)
        {
            Subscribe(owner);
        }

        private void Subscribe(CardAndHealthEntityOwnerData owner)
        {
            owner.Health
                .Subscribe(hp =>
                {
                    if (hp <= 0)
                        OnEntityDied(owner);
                })
                .AddTo(_disposables);
        }
        
        private void OnEntityDied(CardAndHealthEntityOwnerData owner)
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(owner);
            if (playerData == null) return;

            var cardOnBoard = playerData.CardsInBoard.CurrentValue
                .FirstOrDefault(c => c != null && c.id == owner.CardId);
        
            if (cardOnBoard != null)
                playerData.RemoveCardFromBoard(cardOnBoard);
        }
        
        public void Dispose() => _disposables.Dispose();
    }
}