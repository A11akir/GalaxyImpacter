// PassiveEffectsLifecycleSystem.cs — добавить метод
using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.Entity.Script
{
    public class PassiveEffectsLifecycleSystem
    {
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly CombatSystem.CombatSystem _combatSystem;
        private readonly PassiveEffectsData _data;
        private List<PassiveEffectBase> _previousList = new();

        public PassiveEffectsLifecycleSystem(
            CardAndHealthEntityOwnerData owner,
            CombatSystem.CombatSystem combatSystem,
            PassiveEffectsData data)
        {
            _owner = owner;
            _combatSystem = combatSystem;
            _data = data;

            data.ActivePassives.Subscribe(HandleChanged);
        }

        private void HandleChanged(List<PassiveEffectBase> currentList)
        {
            var added = currentList.Except(_previousList);
            var removed = _previousList.Except(currentList);

            foreach (var passive in removed)
                passive.Unregister();

            foreach (var passive in added)
                passive.Register(_owner, _combatSystem);

            _previousList = new List<PassiveEffectBase>(currentList);
        }

        public void TickTurnEnd() // ← новый метод, заменяет старый PassiveEffectsData.OnTurnEnd()
        {
            var toRemove = new List<PassiveEffectBase>();

            foreach (var passive in _data.ActivePassives.CurrentValue)
                if (passive.TickTurnEnd())
                    toRemove.Add(passive);

            foreach (var p in toRemove)
                _data.Remove(p); // ← Remove просто меняет список, HandleChanged сам поймает изменение и вызовет Unregister
        }
    }
}