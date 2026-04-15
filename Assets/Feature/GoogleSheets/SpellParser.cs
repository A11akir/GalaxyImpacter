using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
#if UNITY_EDITOR
    public class SpellParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private SpellStatsConfig _spellStatsConfig;
        private readonly List<ISpellStatsData> _targetSO = new();

        public SpellParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.AllSpellStats = new List<SpellStatsConfig>();
            LoadAllCardsSO();
        }

        private void LoadAllCardsSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Feature" });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (so is ISpellStatsData card)
                {
                    _targetSO.Add(card);
                }
            }
        }

        public void Parse(string header, string token)
        {
            switch (header)
            {
                case "Name":
                    _spellStatsConfig = new SpellStatsConfig
                    {
                        Name = token,
                        Values = new List<int>(),
                        Specialization = new List<string>()
                    };
                    _allGameConfig.AllSpellStats.Add(_spellStatsConfig);
                    break;
                case "Cost":
                    if (_spellStatsConfig != null)
                        _spellStatsConfig.Cost = Convert.ToInt32(token);
                    break;              
                case "Rarity":
                    if (_spellStatsConfig != null)
                        _spellStatsConfig.Rarity = token;
                    break;
                case "Description":
                    if (_spellStatsConfig != null)
                        _spellStatsConfig.Description = token;
                    break;
                case "Value1":
                case "Value2":
                case "Value3":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        if (int.TryParse(token, out int value))
                        {
                            _spellStatsConfig.Values.Add(value);
                            Debug.Log($"Добавлено значение {value} для {_spellStatsConfig.Name} из {header}");
                        }
                    }
                    break;

                case "Specialization1":
                case "Specialization2":
                case "Specialization3":
                case "Specialization4":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        _spellStatsConfig.Specialization.Add(token);
                        Debug.Log($"Добавлена специализация {token} для {_spellStatsConfig.Name} из {header}");
                    }
                    break;
            }
        }

        public void ApplyToSO()
        {
            const string path = "Assets/Feature/Card/Resources/Configs";
    
            foreach (var cfg in _allGameConfig.AllSpellStats)
            {
                var so = _targetSO.FirstOrDefault(x => (x as ScriptableObject).name == cfg.Name);

                if (so == null)
                {
                    var newSO = ScriptableObject.CreateInstance<SpellCardData>();
                    string assetPath = $"{path}/{cfg.Name}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    _targetSO.Add(so);
                    Debug.Log($"✅ Created new SpellCardData SO: {cfg.Name}");
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Rarity = cfg.Rarity;
                so.Description = cfg.Description;
                so.Specialization = cfg.Specialization;
                so.Values = cfg.Values;
                so.Level = cfg.Level;

                EditorUtility.SetDirty(so as UnityEngine.Object);
                Debug.Log($"✅ Updated SpellCardData SO: {cfg.Name}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

#endif
}