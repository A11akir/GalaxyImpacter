#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
    public class MinionParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private MinionStatsConfig _minionStatsConfig;
        private readonly List<IMinionStatsData> _targetSO = new();

        public MinionParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.AllMinionStats = new List<MinionStatsConfig>();
            LoadAllCardsSO();
        }

        private void LoadAllCardsSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Feature" });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (so is IMinionStatsData card)
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
                    _minionStatsConfig = new MinionStatsConfig
                    {
                        Name = token,
                        Values = new List<int>(),
                        Specialization = new List<string>()
                    };
                    _allGameConfig.AllMinionStats.Add(_minionStatsConfig);
                    break;
                    
                case "Cost":
                    if (_minionStatsConfig != null)
                        _minionStatsConfig.Cost = Convert.ToInt32(token);
                    break;              
                case "Health":
                    if (_minionStatsConfig != null)
                        _minionStatsConfig.Health = Convert.ToInt32(token);
                    break;
                case "Сhakra":
                    if (_minionStatsConfig != null)
                        _minionStatsConfig.Chakra = Convert.ToInt32(token);
                    break;
                case "HandCardCount":
                    if (_minionStatsConfig != null)
                        _minionStatsConfig.HandCardCount = Convert.ToInt32(token);
                    break;
                case "Rarity":
                    if (_minionStatsConfig != null)
                        _minionStatsConfig.Rarity = token;
                    break;
                case "SpellsList":
                    if (!string.IsNullOrWhiteSpace(token))
                        _minionStatsConfig.SpellNames = token
                            .Split(',')
                            .Select(s => s.Trim())
                            .ToList();
                    break;

                case "Specialization1":
                case "Specialization2":
                case "Specialization3":
                case "Specialization4":
                    if (_minionStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        _minionStatsConfig.Specialization.Add(token);
                        Debug.Log($"Добавлена специализация {token} для {_minionStatsConfig.Name} из {header}");
                    }
                    break;
            }
        }

        public void ApplyToSO()
        {
            const string path = "Assets/Feature/Card/Resources/Configs";

            var allSpellSOs = new Dictionary<string, SpellCardData>();
            string[] guids = AssetDatabase.FindAssets("t:SpellCardData", new[] { "Assets/Feature/Card/Resources/Configs" });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var spellSO = AssetDatabase.LoadAssetAtPath<SpellCardData>(assetPath);
                if (spellSO != null)
                    allSpellSOs[spellSO.name] = spellSO;
            }

            foreach (var cfg in _allGameConfig.AllMinionStats)
            {
                var so = _targetSO.FirstOrDefault(x => (x as ScriptableObject).name == cfg.Name);

                if (so == null)
                {
                    var newSO = ScriptableObject.CreateInstance<MinionCardData>();
                    string assetPath = $"{path}/{cfg.Name}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    _targetSO.Add(so);
                    Debug.Log($"✅ Created new MinionCardData SO: {cfg.Name}");
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Rarity = cfg.Rarity;
                so.Specialization = cfg.Specialization;
                so.Level = cfg.Level;
                so.Health = cfg.Health;
                so.Chakra = cfg.Chakra;
                so.HandCardCount = cfg.HandCardCount;

                so.SpellsList = cfg.SpellNames?
                    .Select(name => allSpellSOs.TryGetValue(name, out var spellSO) ? spellSO : null)
                    .Where(s => s != null)
                    .ToList() ?? new List<SpellCardData>();

                EditorUtility.SetDirty(so as UnityEngine.Object);
                Debug.Log($"✅ Updated MinionCardData SO: {cfg.Name}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif