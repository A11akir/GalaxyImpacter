using System.Linq;
using Feature.Battlefield.Script;
using Feature.CardEffect.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;

namespace Feature.Card.Script
{
    public class CardCastService
    {
        private readonly GameSessionModel _gameSessionModel;
        private readonly BattlefieldSystem _battlefieldSystem;
        private readonly CombatSystem.CombatSystem _combatSystem;
        private readonly CardPoolPickSystem _cardPoolPickSystem;

        public CardCastService(GameSessionModel gameSessionModel,
            BattlefieldSystem battlefieldSystem,
            CombatSystem.CombatSystem combatSystem, CardPoolPickSystem cardPoolPickSystem)
        {
            _gameSessionModel = gameSessionModel;
            _battlefieldSystem = battlefieldSystem;
            _combatSystem = combatSystem;
            _cardPoolPickSystem = cardPoolPickSystem;
        }

        public bool CheckCanCast(CardStatsData card, CardAndHealthEntityOwnerData owner)
        {
            var playerData = _gameSessionModel.GetPlayerDataByOwner(owner);

            if (card.IsHero &&
                playerData.CardsInBoard.CurrentValue.Count > playerData.CardsInBoardMax)
                return false;

            if (owner.Chakra < card.Cost)
                return false;

            return true;
        }

        public void Cast(CardStatsData card, CardAndHealthEntityOwnerData owner, CardAndHealthEntityOwnerData target)
        {
            
            if (!CheckCanCast(card, owner)) return;

            owner.Chakra -= card.Cost;
            
            
            var cardInHand = owner.CardsInHand.CurrentValue.FirstOrDefault(c => c.id == card.id);
            if (cardInHand != null)
            {
                owner.RemoveCardFromHand(cardInHand);
            }
            
            if (card is MinionCardData)
            {
                var playerData = _gameSessionModel.GetPlayerDataByOwner(owner);
                _battlefieldSystem.AddCardInBattlefield(playerData, card);
            }
            else if (card is SpellCardData spell)
            {
                for (int i = 0; i < spell.Effects.Count; i++)
                    spell.Effects[i].Execute(new EffectContext
                    {
                        Caster = owner,
                        Target = target,
                        GameSessionModel = _gameSessionModel,
                        BattlefieldSystem = _battlefieldSystem,
                        CombatSystem = _combatSystem,
                        CardData = spell,
                        ValueIndex = i,
                        CardPoolPickSystem = _cardPoolPickSystem
                    });
            }
        }
    }
}