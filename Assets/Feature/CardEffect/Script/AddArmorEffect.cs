using System;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class AddArmorEffect : CardEffect
    {
        public override void Execute(EffectContext context)
        {
            int armor = context.CardData.Values[context.ValueIndex];

            context.Target.ArmorValue += armor;
        }
    }
}