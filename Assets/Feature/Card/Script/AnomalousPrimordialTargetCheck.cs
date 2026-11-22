using System;
using System.Linq;
using Feature.GameSessionData;

namespace Feature.Card.Script
{
    /// <summary>Цель — существо редкости Аномальный или Примордиал. Герои и Common/Hidden не проходят.</summary>
    [Serializable]
    public class AnomalousPrimordialTargetCheck : TargetCheck
    {
        public override bool CanPick(
            CardAndHealthEntityOwnerData target,
            CardAndHealthEntityOwnerData caster,
            GameSessionModel model)
        {
            var side = model.GetPlayerDataByOwner(target);
            if (side == null) return false;

            // Герой не имеет редкости — не проходит.
            if (target == side.MainHeroEntity()) return false;

            var card = side.CardsInBoard.CurrentValue.FirstOrDefault(c => c != null && c.id == target.CardId);
            if (card == null) return false;

            return card.Rarity == CardRarity.Anomalous || card.Rarity == CardRarity.Primordial;
        }
    }
}
