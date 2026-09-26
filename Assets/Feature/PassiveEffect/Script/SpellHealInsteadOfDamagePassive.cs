using System;
using Feature.CombatSystem;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;

[Serializable]
public class SpellHealInsteadOfDamagePassive : PassiveEffectBase, IDamageReaction
{
    public SpellHealInsteadOfDamagePassive()
    {
        Duration = DurationType.Permanent;
    }

    public override void Register(CardAndHealthEntityOwnerData owner) { }
    public override void Unregister() { }

    public bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage)
    {
        target.HealthValue += finalDamage;
        return true; // урон обработан, стандартное вычитание не нужно
    }

    public override PassiveEffectBase Clone() => new SpellHealInsteadOfDamagePassive { Config = Config };
}