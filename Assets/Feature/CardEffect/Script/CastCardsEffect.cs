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

            int hpBefore = target != null ? target.HealthValue : 0;
            int armorBefore = target != null ? target.ArmorValue : 0;

            Debug.Log($"[CastCards] ═══════════════════════════");
            Debug.Log($"[CastCards] Card   : {spell.Name} (Cost={spell.Cost})");
            Debug.Log($"[CastCards] Caster : {DescribeOwner(ctx.Caster, ctx)}");
            Debug.Log($"[CastCards] Target : {DescribeOwner(target, ctx)} | HP={hpBefore} | Armor={armorBefore}");

            CardEffect.ExecuteAll(castContext, spell.Effects);

            int hpAfter = target != null ? target.HealthValue : 0;
            int armorAfter = target != null ? target.ArmorValue : 0;

            Debug.Log($"[CastCards] Result : {DescribeOwner(target, ctx)} | HP={hpAfter} ({(hpAfter - hpBefore >= 0 ? "+" : "")}{hpAfter - hpBefore}) | Armor={armorAfter} ({(armorAfter - armorBefore >= 0 ? "+" : "")}{armorAfter - armorBefore})");
            Debug.Log($"[CastCards] ═══════════════════════════");
        }

        private static string DescribeOwner(CardAndHealthEntityOwnerData owner, EffectContext ctx)
        {
            if (owner == null) return "null";

            var playerData = ctx.GameSessionModel.GetPlayerDataByOwner(owner);
            bool isHero = playerData != null && owner == playerData.MainHeroEntity();

            if (isHero)
                return $"Hero({owner._heroName})";

            return $"Minion({owner.CardId})";
        }
    }
}
