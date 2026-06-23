
using System;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddPassiveEffect : CardEffect
    {
        [SerializeReference] private PassiveEffectBase _passiveTemplate;

// AddPassiveEffect.cs
        public override void Execute(EffectContext ctx)
        {
            var targets = ResolveTargets(ctx);
            Debug.Log($"[AddPassiveEffect] targets count={targets.Count}");

            foreach (var target in targets)
            {
                var passive = ResolvePassive(ctx, target);
                Debug.Log($"[AddPassiveEffect] passive type={passive.GetType().Name}, target={target._heroName}");

                if (passive is ICardContextConsumer consumer)
                {
                    consumer.OnAppliedFromCard(ctx);
                    Debug.Log("[AddPassiveEffect] OnAppliedFromCard called");
                }
                else
                {
                    Debug.Log("[AddPassiveEffect] passive is NOT ICardContextConsumer");
                }
            }
        }

        private PassiveEffectBase ResolvePassive(EffectContext ctx, CardAndHealthEntityOwnerData owner)
        {
            if (_passiveTemplate is not IStackablePassive)
                return owner.PassiveEffects.Create(_passiveTemplate);

            return owner.PassiveEffects.Find(_passiveTemplate.GetType())
                   ?? owner.PassiveEffects.Create(_passiveTemplate);
        }
    }
}