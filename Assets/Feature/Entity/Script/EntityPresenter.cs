using Feature.CardEffect.Script;
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
        private readonly PassiveEffectsPresenter _passiveEffectsPresenter;

        public EntityPresenter(
            CardAndHealthEntityOwnerData owner,
            IHealthView healthView,
            PassiveEffectsContainerView passiveEffectsView)
        {
            _owner = owner;
            _healthView = healthView;

            _owner.Health
                .Subscribe(hp => _healthView.SetHealth(hp))
                .AddTo(_disposables);

            if (passiveEffectsView != null)
                _passiveEffectsPresenter = new PassiveEffectsPresenter(passiveEffectsView, _owner.PassiveEffects);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _passiveEffectsPresenter?.Dispose();
        }
    }
}