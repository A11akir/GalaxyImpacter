using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class TargetingSystem
    {
        private readonly Dictionary<GameObject, CardAndHealthEntityOwnerData> _targetsData = new();

        public void RegisterTarget(GameObject gameObject, CardAndHealthEntityOwnerData owner)
        {
            _targetsData[gameObject] = owner;
        }

        public CardAndHealthEntityOwnerData GetTarget(GameObject gameObject)
        {
            return _targetsData.TryGetValue(gameObject, out var owner) ? owner : null;
        }
    }
}