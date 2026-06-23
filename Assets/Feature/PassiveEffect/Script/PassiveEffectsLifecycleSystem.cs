using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.Entity.Script;
using Feature.GameSessionData;
using R3;

namespace Feature.PassiveEffect.Script
{
    public class PassiveEffectsLifecycleSystem
    {
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CombatSystem.CombatSystem _combatSystem;
        private readonly PassiveEffectsData _data;
        private List<PassiveEffectBase> _previousList = new();
        private readonly CardCastService _cardCastService;
        private readonly CardPoolPickSystem _cardPoolPickSystem;

        public PassiveEffectsLifecycleSystem(
            CardAndHealthEntityOwnerData owner,
            CombatSystem.CombatSystem combatSystem,
            PassiveEffectsData data, CardCastService cardCastService, CardPoolPickSystem cardPoolPickSystem)
        {
            _owner = owner;
            _combatSystem = combatSystem;
            _data = data;
            _cardCastService = cardCastService;
            _cardPoolPickSystem = cardPoolPickSystem;

            data.ActivePassives.Subscribe(HandleChanged);
        }

        private void HandleChanged(List<PassiveEffectBase> currentList)
        {
            var added = currentList.Except(_previousList);
            var removed = _previousList.Except(currentList);
            
            foreach (var passive in removed)
                passive.Unregister();

            foreach (var passive in added)
                passive.Register(_owner, _combatSystem, _cardCastService, _cardPoolPickSystem);

            _previousList = new List<PassiveEffectBase>(currentList);
        }
    }
}