using System;
using Feature.GameSessionData;

namespace Feature.CardEffect.Script
{
    public interface IDynamicCostValueSource
    {
        IDisposable Subscribe(CardAndHealthEntityOwnerData owner, GameSessionPlayerData side, Action<int> onValueChanged);
    }
}
