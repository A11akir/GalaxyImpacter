using System.Collections.Generic;
using Feature.Card.Script;
using UnityEngine;

namespace Feature.GoogleSheets
{
    [CreateAssetMenu(fileName = "SpellCard", menuName = "Configs/Card/Spell Card", order = 2)]
    public class SpellCardData : CardStatsData, ISpellStatsData
    {
        [SerializeField] private List<int> _values;
        [SerializeField] private string _description;
        [SerializeField] public TargetSpellType targetSpellType;
        public List<int> Values { get => _values; set => _values = value; }
        public string Description { get => _description; set => _description = value; }
    }
}