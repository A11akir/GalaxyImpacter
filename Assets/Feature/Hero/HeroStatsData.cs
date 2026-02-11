using Feature.GoogleSheets;
using UnityEngine;

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