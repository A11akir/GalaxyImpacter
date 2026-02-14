using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
    public class StatsMinionParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private HeroStatsConfig _currentStatsMinionConfig;
        
        private readonly List<IHeroStatsData> _targetSO = new();

        public StatsMinionParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.HeroStats = new List<HeroStatsConfig>();
            
            LoadAllMinionSO();
        }

        private void LoadAllMinionSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] {"Assets/Feature"});
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (so is IHeroStatsData minion)
                {
                    _targetSO.Add(minion);
                }
            }
        }

        public void Parse(string header, string token)
        {
            switch (header)
            {
                case "Name":
                    _currentStatsMinionConfig = new HeroStatsConfig() { HeroName = token };
                    _allGameConfig.HeroStats.Add(_currentStatsMinionConfig);
                    break;
                case "HeroPowerCost":
                    _currentStatsMinionConfig.HeroPowerCost = Convert.ToInt32(token);
                    break;

                case "Health":
                    _currentStatsMinionConfig.Health = Convert.ToInt32(token);
                    break;  
            }
        }

        public void ApplyToSO()
        {
            foreach (var cfg in _allGameConfig.HeroStats)
            {
                var so = _targetSO
                    .FirstOrDefault(x => (x as ScriptableObject).name == cfg.HeroName);
                if (so == null)
                {
                    Debug.LogWarning($"SO not found for minion: {cfg.HeroName}");
                    continue;
                }

                so.Name = cfg.HeroName;
                so.Rarity = cfg.Health;
                so.Cost = cfg.HeroPowerCost;

                EditorUtility.SetDirty(so as UnityEngine.Object);
                Debug.Log($"✅ Updated SO: {cfg.HeroName}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
