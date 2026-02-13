using Feature.GoogleSheets;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Hero
{
    [CreateAssetMenu(
        fileName = "HeroStatsData",
        menuName = "Configs/Hero/Hero Stats Data",
        order = 1)]
    public class HeroStatsData : ScriptableObject, IHeroStatsData
    {
        [SerializeField] private int _heroPowerCost;
        [SerializeField] private string _name;
        [SerializeField] private int _health;
        [SerializeField] private Sprite _iconImage;
        
        public Sprite IconImage
        {
            get => _iconImage;
            set => _iconImage = value;
        }        
        public int HeroPowerCost
        {
            get => _heroPowerCost;
            set => _heroPowerCost = value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int Health
        {
            get => _health;
            set => _health = value;
        }
    }
}