using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.Card.Script
{
    public class TargetingSystem
    {
        private readonly Dictionary<GameObject, CardAndHealthEntityOwnerData> _targetsData = new();
        private readonly GameSessionModel _gameSessionModel;

        public bool IsPreparePhase { get; set; }

        public TargetingSystem(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }

        public void RegisterTarget(GameObject gameObject, CardAndHealthEntityOwnerData owner)
        {
            _targetsData[gameObject] = owner;
        }

        public CardAndHealthEntityOwnerData GetTarget(GameObject gameObject, CardAndHealthEntityOwnerData caster)
        {
            if (!_targetsData.TryGetValue(gameObject, out var target)) return null;
            return IsValidTarget(target, caster) ? target : null;
        }

        public void UnregisterTarget(GameObject gameObject)
        {
            _targetsData.Remove(gameObject);
        }

        private bool IsValidTarget(CardAndHealthEntityOwnerData target, CardAndHealthEntityOwnerData caster)
        {
            if (!IsPreparePhase) return true;
            return _gameSessionModel.AreAllies(caster, target);
        }
    }
}