using System;
using Feature.GameSessionData;
using R3;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class MissingHealthValueSource : IDynamicCostValueSource
    {
        public IDisposable Subscribe(CardAndHealthEntityOwnerData owner, GameSessionPlayerData side, Action<int> onValueChanged) =>
            owner.Health.Subscribe(hp => onValueChanged(Mathf.Max(0, owner.MaxHealth - hp)));
    }
}
