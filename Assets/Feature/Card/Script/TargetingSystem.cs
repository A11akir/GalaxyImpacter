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

        public CardAndHealthEntityOwnerData GetTarget(GameObject gameObject, CardAndHealthEntityOwnerData caster) =>
            GetTarget(gameObject, caster, TargetType.AnyTarget);

        public CardAndHealthEntityOwnerData GetTarget(
            GameObject gameObject, CardAndHealthEntityOwnerData caster, TargetType targetType, TargetCheck targetCheck = null)
        {
            if (!_targetsData.TryGetValue(gameObject, out var target)) return null;
            if (!IsValidTarget(target, caster, targetType)) return null;
            if (targetCheck != null && !targetCheck.CanPick(target, caster, _gameSessionModel)) return null;

            return target;
        }

        public void UnregisterTarget(GameObject gameObject)
        {
            _targetsData.Remove(gameObject);
        }

        private bool IsValidTarget(
            CardAndHealthEntityOwnerData target, CardAndHealthEntityOwnerData caster, TargetType targetType)
        {
            bool isAlly = _gameSessionModel.AreAllies(caster, target);
            
            switch (targetType)
            {
                case TargetType.EnemyTarget: return !isAlly;
                case TargetType.AllyTarget: return isAlly;
                case TargetType.OtherTarget: return target != caster;
            }
            
            if (!IsPreparePhase) return true;
            return isAlly;
        }
    }
}
