using System;
using Feature.GameSessionData;
using R3;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class HealthValueSource : IDynamicCostValueSource
    {
        public IDisposable Subscribe(CardAndHealthEntityOwnerData owner, Action<int> onValueChanged) =>
            owner.Health.Subscribe(onValueChanged);
    }
}