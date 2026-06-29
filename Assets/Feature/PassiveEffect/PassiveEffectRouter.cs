using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.Hero.Script;
using Feature.PassiveEffect.Script;

namespace Feature.PassiveEffect
{
    /// <summary>
    /// Решает, в какую вьюху должен попасть конкретный пассивный эффект:
    /// в окно силы героя (permanent-пассивки героя) или в обычный контейнер пассивных эффектов.
    /// Не хранит состояние, не следит за списками — только маршрутизация одного эффекта за раз.
    /// </summary>
    public class PassiveEffectRouter
    {
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly GameSessionModel _gameSessionModel;
        private readonly PassiveEffectsPresenter _passiveEffectsPresenter;
        private readonly HeroPowerPresenter _heroPowerPresenter;

        public PassiveEffectRouter(
            CardAndHealthEntityOwnerData owner,
            GameSessionModel gameSessionModel,
            PassiveEffectsData data,
            PassiveEffectsPresenter passiveEffectsPresenter,
            HeroPowerPresenter heroPowerPresenter)
        {
            _owner = owner;
            _gameSessionModel = gameSessionModel;
            _passiveEffectsPresenter = passiveEffectsPresenter;
            _heroPowerPresenter = heroPowerPresenter;

            data.PassiveAdded.Subscribe(HandleAdded);
            data.PassiveRemoved.Subscribe(HandleRemoved);
        }

        private void HandleAdded(PassiveEffectBase passive)
        {
            if (RoutesToHeroPower(passive))
                _heroPowerPresenter.HandlePassiveAdded(passive, passive.SourceCard, _owner);
            else
                _passiveEffectsPresenter?.HandlePassiveAdded(passive);
        }

        private void HandleRemoved(PassiveEffectBase passive)
        {
            if (RoutesToHeroPower(passive))
                _heroPowerPresenter.HandlePassiveRemoved(passive);
            else
                _passiveEffectsPresenter?.HandlePassiveRemoved(passive);
        }

        private bool RoutesToHeroPower(PassiveEffectBase passive) =>
            passive.Duration == DurationType.Permanent && IsHero();

        private bool IsHero()
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(_owner);
            return playerData != null && _owner == playerData.MainHeroEntity();
        }
    }
}