using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class EntityPresenter
    {
        private readonly IEntityView _entityView;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CompositeDisposable _disposables = new();
        private PassiveEffectsLifecycleSystem _lifecycleSystem;
        private PassiveEffectsPresenter _passiveEffectsPresenter;

        public EntityPresenter(
            CardAndHealthEntityOwnerData owner,
            IEntityView entityView,
            PassiveEffectsContainerView passiveEffectsView)
        {
            _owner = owner;
            _entityView = entityView;

            InitHealth();

            InitPassiveEffects(owner, passiveEffectsView);
        }

        private void InitPassiveEffects(CardAndHealthEntityOwnerData owner, PassiveEffectsContainerView passiveEffectsView)
        {
            _lifecycleSystem = new PassiveEffectsLifecycleSystem(owner, owner.PassiveEffects);

            if (passiveEffectsView != null)
                _passiveEffectsPresenter = new PassiveEffectsPresenter(passiveEffectsView, owner.PassiveEffects);
        }

        private void InitHealth()
        {
            _owner.Health
                .Subscribe(hp => _entityView.SetHealth(hp))
                .AddTo(_disposables);

            _owner.Armor
                .Subscribe(armor => _entityView.SetArmor(armor))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}