using System.Collections.Generic;
using Feature.GameSessionData;
using Feature.GoogleSheets;

namespace Feature.CardEffect.Script
{
    public class HandCostModifierSystem
    {
        private readonly Dictionary<CardStatsData, int> _modifiers = new();

        public void ApplyTemporaryModifier(CardAndHealthEntityOwnerData owner, CardStatsData card, int delta)
        {
            if (_modifiers.ContainsKey(card))
                _modifiers[card] += delta;
            else
                _modifiers[card] = delta;

            card.Cost += delta; // напрямую меняем экземпляр (он уже Instantiate-нутый, уникальный)
        }

        public void ClearAllModifiers()
        {
            foreach (var pair in _modifiers)
                pair.Key.Cost -= pair.Value; // откатываем изменение

            _modifiers.Clear();
        }
    }
}