using System.Collections.Generic;
using Feature.CardEffect.Script;
using Feature.GoogleSheets;
using UnityEngine;

namespace Feature.Items.Scripts
{
    [CreateAssetMenu(fileName = "SpellCard", menuName = "Configs/Card/Spell Card", order = 3)]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private int _goldCost;
        [SerializeField] private string _name;
        [SerializeField] private List<int> _values;
        [SerializeField] private List<ItemEffectSO> _effects;
        [SerializeField] private Sprite _iconImage;
        [SerializeField] private string _description;
        
        public string Description { get => _description; set => _description = value; }
        public List<int> Values { get => _values; set => _values = value; }
        public string ItemName { get => _name; set => _name = value; }
        public int GoldCost { get => _goldCost; set => _goldCost = value; }
        public Sprite IconImage { get => _iconImage; set => _iconImage = value; }
        public List<ItemEffectSO> Effects => _effects;
    }
}