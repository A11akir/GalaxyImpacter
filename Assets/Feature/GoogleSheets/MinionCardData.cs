using System.Collections.Generic;
using Feature.Card.Script;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Feature.GoogleSheets
{
    [CreateAssetMenu(fileName = "MinionCard", menuName = "Configs/Card/Minion Card", order = 1)]
    public class MinionCardData : CardStatsData, IMinionStatsData
    {
        [SerializeField] private int _health;
        [SerializeField] private int _chakra;
        [SerializeField] private int _handCardCount;
        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true, ShowItemCount = true)]
        private List<SpellCardData> _spellsList;

        public override bool IsHero => _health > 0;

        
        public int Health { get => _health; set => _health = value; }
        public int Chakra { get => _chakra; set => _chakra = value; }
        public int HandCardCount { get => _handCardCount; set => _handCardCount = value; }
        public List<SpellCardData> SpellsList { get => _spellsList; set => _spellsList = value; }
    }
}
