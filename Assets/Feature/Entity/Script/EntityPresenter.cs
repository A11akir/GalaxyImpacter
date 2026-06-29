// EntityPresenter.cs — убираем лишний параметр passiveEffectRouter
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.Hero.Script;
using Feature.PassiveEffect;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class EntityPresenter
    {
        private readonly IEntityView _entityView;
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CompositeDisposable _disposables = new();
        private readonly PassiveEffectsLifecycleSystem _lifecycleSystem;
        private readonly PassiveEffectsPresenter _passiveEffectsPresenter;
        private readonly PassiveEffectRouter _passiveEffectRouter;

        public EntityPresenter(
            CardAndHealthEntityOwnerData owner,
            IEntityView entityView,
            PassiveEffectsContainerView passiveEffectsView,
            HeroPowerPresenter heroPowerPresenter,
            GameSessionModel gameSessionModel)
        {
            _owner = owner;
            _entityView = entityView;

            InitHealth();

            _passiveEffectsPresenter = passiveEffectsView != null
                ? new PassiveEffectsPresenter(passiveEffectsView)
                : null;

            _passiveEffectRouter = new PassiveEffectRouter(owner, gameSessionModel, owner.PassiveEffects, _passiveEffectsPresenter, heroPowerPresenter);
            _lifecycleSystem = new PassiveEffectsLifecycleSystem(owner, owner.PassiveEffects);
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

        public void Dispose() => _disposables.Dispose();
    }
}