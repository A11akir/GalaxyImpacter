using System.Collections.Generic;
using Feature.GoogleSheets;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Feature.Card.Script
{
    [CreateAssetMenu(
        fileName = "HeroStatsData",
        menuName = "Configs/Card/Card Stats Data",
        order = 1)]
    public class CardStatsData : ScriptableObject, ICardStatsData
    {
        [SerializeField] private int _cost;
        [SerializeField] private string _name;
        [SerializeField] private string _rarity;
        [SerializeField] private Sprite _iconImage;
        [SerializeField] private List<string> _specialization;
        [SerializeField] private int _level;
        [SerializeField] public TargetType targetType;

        public string id = System.Guid.NewGuid().ToString();
        public virtual bool IsHero => false;
        public string Name { get => _name; set => _name = value; }
        public int Cost { get => _cost; set => _cost = value; }
        public string Rarity { get => _rarity; set => _rarity = value; }
        public List<string> Specialization { get => _specialization; set => _specialization = value; }
        public int Level { get => _level; set => _level = value; }
        public Sprite IconImage { get => _iconImage; set => _iconImage = value; }

    }
}