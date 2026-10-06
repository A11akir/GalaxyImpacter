// PassiveCardEffect.cs
using System;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public abstract class PassiveCardEffect
    {
        public abstract IDisposable BindPassiveEffect(CardAndHealthEntityOwnerData owner, CardStatsData card);
    }
}