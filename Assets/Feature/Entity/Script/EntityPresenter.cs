// EntityPresenter.cs — убираем CombatSystem, CardCastService, CardPoolPickSystem из конструктора

using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.Health;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class EntityPresenter
    {
        private readonly IHealthView _healthView;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CompositeDisposable _disposables = new();
        private readonly PassiveEffectsLifecycleSystem _lifecycleSystem;
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

            _owner.Armor
                .Subscribe(armor => _healthView.SetArmor(armor))
                .AddTo(_disposables);

            _lifecycleSystem = new PassiveEffectsLifecycleSystem(owner, owner.PassiveEffects);

            if (passiveEffectsView != null)
                _passiveEffectsPresenter = new PassiveEffectsPresenter(passiveEffectsView, owner.PassiveEffects);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}