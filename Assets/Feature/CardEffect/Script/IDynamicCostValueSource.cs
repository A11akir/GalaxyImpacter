using System;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    public interface IDynamicCostValueSource
    {
        IDisposable Subscribe(CardAndHealthEntityOwnerData owner, Action<int> onValueChanged);
    }
}