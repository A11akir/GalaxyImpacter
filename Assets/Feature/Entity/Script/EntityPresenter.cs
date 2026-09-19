using Feature.CardEffect.Script;
using Feature.CombatSystem;
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
        private readonly GameEventDispatcher _eventDispatcher;

        public EntityPresenter(
            CardAndHealthEntityOwnerData owner,
            IEntityView entityView,
            PassiveEffectsContainerView passiveEffectsView,
            HeroPowerPresenter heroPowerPresenter,
            GameSessionModel gameSessionModel,
            GameEventDispatcher eventDispatcher)
        {
            _owner = owner;
            _entityView = entityView;
            _eventDispatcher = eventDispatcher;

            InitHealth();
            InitDamagePopup();

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

        private void InitDamagePopup()
        {
            _eventDispatcher.Subscribe<DamageDealtInfo>(HandleDamage);
        }

        private void HandleDamage(DamageDealtInfo info)
        {
            if (info.Target != _owner) return;
            _entityView.ShowDamage(info.Amount);
        }

        public void Dispose()
        {
            _eventDispatcher.Unsubscribe<DamageDealtInfo>(HandleDamage);
            _disposables.Dispose();
        }
    }
}