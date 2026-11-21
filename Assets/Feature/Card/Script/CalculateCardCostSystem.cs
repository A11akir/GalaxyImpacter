using System.Collections.Generic;
using UnityEngine;

namespace Feature.Card.Script
{
    /// <summary>
    /// Считает итоговую цену карты: базовая цена минус все скидки.
    /// Менять цену карты можно только через эту систему.
    /// </summary>
    public class CalculateCardCostSystem
    {
        private readonly Dictionary<CardStatsData, Dictionary<object, int>> _discounts = new();

        public void SetDiscount(CardStatsData card, object source, int discount)
        {
            if (!_discounts.TryGetValue(card, out var cardDiscounts))
            {
                cardDiscounts = new Dictionary<object, int>();
                _discounts[card] = cardDiscounts;
            }

            cardDiscounts[source] = discount;
            Recalculate(card);
        }

        public void RemoveDiscount(CardStatsData card, object source)
        {
            if (!_discounts.TryGetValue(card, out var cardDiscounts)) return;
            if (!cardDiscounts.Remove(source)) return;

            Recalculate(card);
        }

        private void Recalculate(CardStatsData card)
        {
            int discount = 0;

            foreach (var value in _discounts[card].Values)
                discount += value;

            card.SetCost(Mathf.Max(0, card.BaseCost - discount));
        }
    }
}
