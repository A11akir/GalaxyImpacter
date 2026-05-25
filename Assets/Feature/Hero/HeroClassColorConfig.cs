using System.Collections.Generic;
using UnityEngine;

namespace Feature.Hero
{
    [CreateAssetMenu(fileName = "HeroClassColor", menuName = "Configs/Hero/ColorConfig", order = 1)]
    public class HeroClassColorConfig : ScriptableObject
    {
        [System.Serializable]
        public class HeroClassColorEntry
        {
            public AllHeroClass HeroClass;
            public Color Color;
        }

        [SerializeField] private List<HeroClassColorEntry> _entries = new()
        {
            new HeroClassColorEntry { HeroClass = AllHeroClass.Assassin,  Color = new Color(0.5f, 0.5f, 0.5f) },       // Серый
            new HeroClassColorEntry { HeroClass = AllHeroClass.Warrior,   Color = new Color(0.9f, 0.4f, 0.2f) },       // Бежево-красный
            new HeroClassColorEntry { HeroClass = AllHeroClass.WindMage,  Color = new Color(0.7f, 0.8f, 0.9f) },       // Бежево-голубой
            new HeroClassColorEntry { HeroClass = AllHeroClass.FireMage,  Color = new Color(0.9f, 0.1f, 0.1f) },       // Алый
            new HeroClassColorEntry { HeroClass = AllHeroClass.Monster,   Color = new Color(0.5f, 0.7f, 0.4f) },       // Бежево-зеленый
            new HeroClassColorEntry { HeroClass = AllHeroClass.EarthMage, Color = new Color(0.6f, 0.5f, 0.35f) },      // Бежево-коричневый
            new HeroClassColorEntry { HeroClass = AllHeroClass.Alchemist, Color = new Color(0.4f, 0.05f, 0.05f) },     // Темно-красный
            new HeroClassColorEntry { HeroClass = AllHeroClass.WaterMage, Color = new Color(0.4f, 0.55f, 0.75f) },     // Бежево-синий
            new HeroClassColorEntry { HeroClass = AllHeroClass.Explorer,  Color = new Color(0.8f, 0.75f, 0.4f) },      // Бежево-желтый
        };

        private Dictionary<AllHeroClass, Color> _colorMap;

        private void OnEnable()
        {
            BuildMap();
        }

        private void BuildMap()
        {
            _colorMap = new Dictionary<AllHeroClass, Color>();
            foreach (var entry in _entries)
                _colorMap[entry.HeroClass] = entry.Color;
        }

        public Color GetColor(AllHeroClass heroClass)
        {
            if (_colorMap == null) BuildMap();
            return _colorMap.TryGetValue(heroClass, out var color) ? color : Color.white;
        }
    }
}