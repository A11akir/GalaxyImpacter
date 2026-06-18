using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using UnityEngine;

namespace Feature.GoogleSheets
{
    [CreateAssetMenu(fileName = "SpellCard", menuName = "Configs/Card/Spell Card", order = 2)]
    public class SpellCardData : CardStatsData, ISpellStatsData
    {
        [SerializeField] private List<int> _values;
        [SerializeField] private string _description;
        [SerializeReference] private List<CardEffect.Script.CardEffect> _effects;


        public List<CardEffect.Script.CardEffect> Effects => _effects;
        public List<int> Values { get => _values; set => _values = value; }
        public string Description { get => _description; set => _description = value; }
        public string Type { get; set; }
        public string MinionNameOwner { get; set; }

        public bool IsPassive => _effects != null && _effects.Count > 0 &&
                                 _effects.TrueForAll(e => e is CardEffect.Script.AddPassiveEffect);
    }
}