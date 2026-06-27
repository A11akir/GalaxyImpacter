// PassiveCardEffect.cs
using System;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public abstract class PassiveCardEffect
    {
        public abstract IDisposable Activate(CardAndHealthEntityOwnerData owner, CardStatsData card, Action onChanged);
    }
}