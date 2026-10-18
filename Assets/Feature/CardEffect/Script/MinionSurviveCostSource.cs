using System;
using System.Collections.Generic;
using System.Linq;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using R3;
using UnityEngine;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Отдаёт, сколько ходов требуемое существо уже выжило на столе (0 — если его нет).
    /// </summary>
    [Serializable]
    public class MinionSurviveCostSource : IDynamicCostValueSource
    {
        [SerializeField] private CardStatsData _requiredMinion;

        public IDisposable Subscribe(CardAndHealthEntityOwnerData owner, GameSessionPlayerData side, Action<int> onValueChanged)
        {
            var composite = new CompositeDisposable();
            var inner = new SerialDisposable();
            composite.Add(inner);

            side.CardsInBoard.Subscribe(board =>
            {
                inner.Disposable = null;

                var survivor = FindSurvivor(side, board);
                if (survivor != null)
                    inner.Disposable = survivor.TurnsOnBoard.Subscribe(onValueChanged);
                else
                    onValueChanged(0);
            }).AddTo(composite);

            return composite;
        }

        private CardAndHealthEntityOwnerData FindSurvivor(GameSessionPlayerData side, List<MinionCardData> board)
        {
            if (_requiredMinion == null) return null;

            // На доске лежат копии с новым GUID, поэтому сравниваем по имени.
            var card = board.FirstOrDefault(c => c != null && c.Name == _requiredMinion.Name);
            return card == null
                ? null
                : side.CardAndHealthEntityOwners.FirstOrDefault(o => o.CardId == card.id);
        }
    }
}
