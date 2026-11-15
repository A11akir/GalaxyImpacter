using System;
using Feature.CardEffect.Script;

namespace Feature.PassiveEffect.Script
{
    public interface ICritModifier
    {
        void Apply(CritState state, EffectContext context);
    }
    
    [Serializable]
    public class CritState
    {
        public int ChancePercent = 100;
        public int DamagePercent;
    }
}
