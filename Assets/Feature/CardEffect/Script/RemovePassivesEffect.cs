using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Снимает с целей (TargetSelection) пассивки, перечисленные в списке шаблонов.
    /// Поиск по точному типу — как в AddPassiveEffect.
    /// </summary>
    [Serializable]
    public class RemovePassivesEffect : CardEffect
    {
        [SerializeReference] private List<PassiveEffectBase> _removeTemplates = new();

        public override void Execute(EffectContext context)
        {
            foreach (var target in ResolveTargets(context))
            {
                if (target == null) continue;

                RemoveListedPassives(target);
            }
        }

        private void RemoveListedPassives(CardAndHealthEntityOwnerData target)
        {
            var passives = new List<PassiveEffectBase>(target.PassiveEffects.ActivePassives.CurrentValue);

            foreach (var template in _removeTemplates)
            {
                if (template == null) continue;

                var templateType = template.GetType();

                foreach (var passive in passives)
                {
                    if (passive == null || passive.GetType() != templateType) continue;

                    target.PassiveEffects.Remove(passive);
                }
            }
        }
    }
}
