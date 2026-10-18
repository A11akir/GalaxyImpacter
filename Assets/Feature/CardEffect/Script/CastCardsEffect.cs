using System;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.GoogleSheets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feature.CardEffect.Script
{
    /// <summary>
    /// Кастует случайную карту по фильтру. Пул целей задаётся через TargetSelection
    /// (как у обычных эффектов), но тут он определяет, куда упадёт КАРТА.
    /// Карты без цели (All / BoardPlace) кастуются как обычно.
    /// </summary>
    [Serializable]
    public class CastCardsEffect : CardEffect
    {
        [FormerlySerializedAs("_query")] [SerializeField] private CardPickFilter filter = new();

        public override void Execute(EffectContext ctx)
        {
            var card = ctx.CardPoolPickSystem.PickInstance(filter, ctx);
            if (card == null) return;

            if (card is SpellCardData spell)
                CastSpell(spell, ctx, PickRandomTarget(spell, ctx));
            else if (card is MinionCardData)
            {
                var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(ctx.Caster);
                if (playerData != null)
                    ctx.BattlefieldSystem.AddCardInBattlefield(playerData, card);
            }
        }

        private CardAndHealthEntityOwnerData PickRandomTarget(SpellCardData spell, EffectContext ctx)
        {
            // Карты, которым цель не нужна, кастуются без цели.
            if (spell.TargetType == TargetType.All || spell.TargetType == TargetType.BoardPlace)
                return null;

            var pool = GetTargetsPool(ctx);
            return pool.Count == 0 ? null : pool[UnityEngine.Random.Range(0, pool.Count)];
        }

        private static void CastSpell(SpellCardData spell, EffectContext ctx, CardAndHealthEntityOwnerData target)
        {
            var castContext = new EffectContext
            {
                Caster = ctx.Caster,
                Target = target,
                GameSessionModel = ctx.GameSessionModel,
                BattlefieldSystem = ctx.BattlefieldSystem,
                CombatSystem = ctx.CombatSystem,
                CardData = spell,
                CurrentEffectsList = spell.Effects,
                CardPoolPickSystem = ctx.CardPoolPickSystem,
                TurnEffectQueue = ctx.TurnEffectQueue,
            };

            CardEffect.ExecuteAll(castContext, spell.Effects);
        }
    }
}
