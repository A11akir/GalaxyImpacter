using System.Collections.Generic;
using System.Linq;
using Feature.Entity.Script;
using Feature.GameSessionData;
using Feature.Hero.Script;
using Feature.PassiveEffect.Script;
using R3;

namespace Feature.PassiveEffect
{
    public class PassiveEffectRouter
    {
        private readonly CardAndHealthEntityOwnerData _owner;
        private readonly GameSessionModel _gameSessionModel;
        private readonly PassiveEffectsPresenter _passiveEffectsPresenter;
        private readonly HeroPowerPresenter _heroPowerPresenter;
        private List<PassiveEffectBase> _previousList = new();

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

            data.ActivePassives.Subscribe(HandleChanged);
        }

        private bool IsHero()
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(_owner);
            return playerData != null && _owner == playerData.MainHeroEntity();
        }

        private void HandleChanged(List<PassiveEffectBase> currentList)
        {
            var added = currentList.Except(_previousList);
            var removed = _previousList.Except(currentList);

            bool isPermanentRoutedToHeroPower = IsHero();

            foreach (var passive in removed)
            {
                if (passive.Duration == DurationType.Permanent && isPermanentRoutedToHeroPower)
                    _heroPowerPresenter.HandlePassiveRemoved(passive);
                else
                    _passiveEffectsPresenter?.HandlePassiveRemoved(passive);
            }
            foreach (var passive in added)
            {
                if (passive.Duration == DurationType.Permanent && isPermanentRoutedToHeroPower)
                {
                    _heroPowerPresenter.HandlePassiveAdded(passive, passive.SourceCard, _owner); // ← добавили _owner
                }
                else
                {
                    _passiveEffectsPresenter?.HandlePassiveAdded(passive);
                }
            }

            _previousList = new List<PassiveEffectBase>(currentList);
        }
    }
}