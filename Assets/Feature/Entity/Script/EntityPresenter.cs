using Feature.Card.Script;
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
        private readonly CardCastService _cardCastService;
        private readonly CardPoolPickSystem _cardPoolPickSystem;

        public EntityPresenter(
            CardAndHealthEntityOwnerData owner,
            IHealthView healthView,
            PassiveEffectsContainerView passiveEffectsView,
            CombatSystem.CombatSystem combatSystem,
            CardCastService cardCastService, CardPoolPickSystem cardPoolPickSystem)
        {
            _owner = owner;
            _healthView = healthView;
            _cardCastService = cardCastService;
            _cardPoolPickSystem = cardPoolPickSystem;


            _owner.Health
                .Subscribe(hp => _healthView.SetHealth(hp))
                .AddTo(_disposables);


            _owner.Armor
                .Subscribe(armor => _healthView.SetArmor(armor))
                .AddTo(_disposables);


            _lifecycleSystem = new PassiveEffectsLifecycleSystem(
                owner,
                combatSystem,
                owner.PassiveEffects,
                cardCastService, cardPoolPickSystem
            );


            if (passiveEffectsView != null)
                _passiveEffectsPresenter =
                    new PassiveEffectsPresenter(
                        passiveEffectsView,
                        owner.PassiveEffects
                    );
        }


        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}