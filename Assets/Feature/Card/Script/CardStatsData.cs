using System.Collections.Generic;
using Feature.GoogleSheets;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Hero
{
    [CreateAssetMenu(
        fileName = "HeroStatsData",
        menuName = "Configs/Card/Card Stats Data",
        order = 1)]
    public class CardStatsData : ScriptableObject, IHeroStatsData
    {
        [SerializeField] private int _cost;
        [SerializeField] private string _name;
        [SerializeField] private int _rarity;
        [SerializeField] private Sprite _iconImage;
        [SerializeField] private List<int> _values;
        [SerializeField] private List<string> _specialization;
        [SerializeField] private int _level;
        
        public List<int> Values
        {
            get => _values;
            set => _values = value;
        }        
        public List<string> Specialization
        {
            get => _specialization;
            set => _specialization = value;
        }
        
        public Sprite IconImage
        {
            get => _iconImage;
            set => _iconImage = value;
        }        
        public int Cost
        {
            get => _cost;
            set => _cost = value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int Rarity
        {
            get => _rarity;
            set => _rarity = value;
        }
        
        public int Level
        {
            get => _level;
            set => _level = value;
        }
    }
}