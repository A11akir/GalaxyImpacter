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
        private List<PassiveEffectBase> _previousList = new();


        public PassiveEffectsLifecycleSystem(
            CardAndHealthEntityOwnerData owner,
            PassiveEffectsData data)
        {
            _owner = owner;

            data.ActivePassives.Subscribe(HandleChanged);
        }

        private void HandleChanged(List<PassiveEffectBase> currentList)
        {
            var added = currentList.Except(_previousList);
            var removed = _previousList.Except(currentList);
            
            foreach (var passive in removed)
                passive.Unregister();

            foreach (var passive in added)
                passive.Register(_owner);

            _previousList = new List<PassiveEffectBase>(currentList);
        }
    }
}