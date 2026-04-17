using Feature.GameSessionData;
using Feature.Health;
using R3;

namespace Feature.Entity.Script
{
    public class EntityPresenter
    {
        private readonly IHealthView _healthView;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CompositeDisposable _disposables = new();

        public EntityPresenter(CardAndHealthEntityOwnerData owner, IHealthView healthView)
        {
            _owner = owner;
            _healthView = healthView;
            
            _owner.Health
                .Subscribe(hp => _healthView.SetHealth(hp))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}