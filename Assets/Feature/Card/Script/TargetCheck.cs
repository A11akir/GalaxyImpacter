using System;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    /// <summary>
    /// Проверка цели карты: можно ли взять этого персонажа в цель.
    /// Примеры: только с неполным здоровьем, только класс Воина.
    /// </summary>
    [Serializable]
    public abstract class TargetCheck
    {
        public abstract bool CanPick(
            CardAndHealthEntityOwnerData target,
            CardAndHealthEntityOwnerData caster,
            GameSessionModel model);
    }
}
