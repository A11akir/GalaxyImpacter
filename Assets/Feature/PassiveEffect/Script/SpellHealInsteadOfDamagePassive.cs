using System;
using Feature.CardEffect.Script;
using Feature.CombatSystem;
using Feature.GameSessionData;

namespace Feature.PassiveEffect.Script
{
    [Serializable]
    public class SpellHealInsteadOfDamagePassive : PassiveEffectBase, IDamageReaction, ICardContextConsumer
    {
        [UnityEngine.SerializeField] private TargetSelectionType _sourceFilter = TargetSelectionType.Self;

        private CardAndHealthEntityOwnerData _owner;
        private GameSessionModel _gameSessionModel;

        public SpellHealInsteadOfDamagePassive()
        {
            Duration = DurationType.Permanent;
        }

        public void OnAppliedFromCard(EffectContext context)
        {
            _gameSessionModel = context.GameSessionModel;
        }

        public override void Register(CardAndHealthEntityOwnerData owner)
        {
            _owner = owner;
        }

        public override void Unregister() { }

        public bool ReactToDamage(CardAndHealthEntityOwnerData target, int finalDamage, CardAndHealthEntityOwnerData source)
        {
            if (!MatchesFilter(source)) return false;

            target.HealthValue += finalDamage;
            return true;
        }

        private bool MatchesFilter(CardAndHealthEntityOwnerData source)
        {
            if (source == null || _gameSessionModel == null) return false;

            var ownerSide = _gameSessionModel.GetPlayerDataByOwner(_owner);
            var sourceSide = _gameSessionModel.GetPlayerDataByOwner(source);

            return _sourceFilter switch
            {
                TargetSelectionType.Self => source == _owner,
                TargetSelectionType.PlayerHero => source == _gameSessionModel.PlayerHero.MainHeroEntity(),
                TargetSelectionType.EnemyHero => source == _gameSessionModel.EnemyHero.MainHeroEntity(),
                TargetSelectionType.PlayerMinion => sourceSide == _gameSessionModel.PlayerHero && source != _gameSessionModel.PlayerHero.MainHeroEntity(),
                TargetSelectionType.EnemyMinion => sourceSide == _gameSessionModel.EnemyHero && source != _gameSessionModel.EnemyHero.MainHeroEntity(),
                TargetSelectionType.Allies => ownerSide != null && sourceSide == ownerSide,
                TargetSelectionType.Enemies => ownerSide != null && sourceSide != null && sourceSide != ownerSide,
                TargetSelectionType.All => true,
                TargetSelectionType.Target => false,
                _ => false
            };
        }

        public override PassiveEffectBase Clone() =>
            new SpellHealInsteadOfDamagePassive { Config = Config, _sourceFilter = _sourceFilter };
    }
}