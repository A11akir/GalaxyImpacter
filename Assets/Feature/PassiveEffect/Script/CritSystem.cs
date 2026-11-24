using Feature.GameSessionData;
using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    public class CritSystem
    {
        public const int MinChancePercent = 10;
        public const int BaseDamagePercent = 100;
        
        public CritResolveResult LastResult { get; private set; } = new();

        public void Resolve(CardAndHealthEntityOwnerData caster, GameSessionModel model, CardStatsData sourceCard)
        {
            var aura = FindAura(caster, model, sourceCard);
            
            int chance = aura != null ? aura.ChancePercent : MinChancePercent;
            int damagePercent = aura != null ? aura.DamagePercent : BaseDamagePercent;

            bool hasCrit = aura != null;
            bool hasMultiplier = false;

            foreach (var passive in caster.PassiveEffects.ActivePassives.CurrentValue)
            {
                if (passive is CritBuff buff)
                {
                    hasCrit = true;
                    chance += buff.ChanceBonus;
                    damagePercent += buff.DamageBonus;
                    if (buff.DoubleChance)
                        hasMultiplier = true;

                    Debug.Log($"[Crit] {caster._heroName}: buff {buff.GetType().Name} damage+{buff.DamageBonus} chance+{buff.ChanceBonus} double={buff.DoubleChance}");
                }
            }

            if (!hasCrit)
            {
                LastResult = new CritResolveResult();
                Debug.Log($"[Crit] {caster._heroName}: критов нет, резолв пропущен");
                return;
            }
            
            if (hasMultiplier)
                chance *= 2;

            bool isCrit = Random.Range(0, 100) < chance;

            LastResult = new CritResolveResult
            {
                IsCrit = isCrit,
                ChancePercent = chance,
                DamagePercent = damagePercent,
                Multiplier = isCrit ? damagePercent / 100f : 1f
            };

            Debug.Log($"[Crit] {caster._heroName}: аура={(aura != null ? "есть" : "нет")} шанс={chance}% урон={damagePercent}% isCrit={isCrit} множитель={LastResult.Multiplier}");
        }

        private static CritDamageAura FindAura(CardAndHealthEntityOwnerData caster, GameSessionModel model, CardStatsData sourceCard)
        {
            foreach (var entity in model.GetAllEntityOwners())
            foreach (var passive in entity.PassiveEffects.ActivePassives.CurrentValue)
                if (passive is CritDamageAura aura && aura.ResolveAura(entity, caster, model, sourceCard))
                    return aura;

            return null;
        }
    }
    
    public class CritResolveResult
    {
        public bool IsCrit;
        public int ChancePercent;
        public int DamagePercent;
        public float Multiplier = 1f;
    }
}
