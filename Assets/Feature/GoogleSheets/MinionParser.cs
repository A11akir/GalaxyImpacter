#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Common;
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
    GLog.Log($"[PARSE] header='{header}', token='{token}' (len={token?.Length ?? -1}), currentMinion={((_minionStatsConfig == null) ? "NULL" : $"'{_minionStatsConfig.Name}'")}");
    
    switch (header)
    {
        case "Name":
            GLog.Log($"  → Creating new MinionStatsConfig with Name='{token}'");
            if (string.IsNullOrWhiteSpace(token))
                return; 
            _minionStatsConfig = new MinionStatsConfig
            {
                Name = token,
                Values = new List<int>(),
                Specialization = new List<string>()
            };
            _allGameConfig.AllMinionStats.Add(_minionStatsConfig);
            GLog.Log($"  → Created, total configs: {_allGameConfig.AllMinionStats.Count}");
            break;
            
        case "Cost":
            GLog.Log($"  → Parsing Cost");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _minionStatsConfig.Cost = Convert.ToInt32(token);
                GLog.Log($"  → Cost set to {_minionStatsConfig.Cost}");
            }
            else
            {
                GLog.Log($"  → Cost skipped (empty token)");
            }
            break;
            
        case "Health":
            GLog.Log($"  → Parsing Health");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _minionStatsConfig.Health = Convert.ToInt32(token);
                GLog.Log($"  → Health set to {_minionStatsConfig.Health}");
            }
            else
            {
                GLog.Log($"  → Health skipped (empty token)");
            }
            break;
            
        case "Chakra":
            if (!string.IsNullOrWhiteSpace(token))
            {
                _minionStatsConfig.Chakra = Convert.ToInt32(token);
                GLog.Log($"  → Chakra set to {_minionStatsConfig.Chakra}");
            }
            else
            {
                GLog.Log($"  → Chakra skipped (empty token)");
            }
            break;
            
        case "HandCardCount":
            GLog.Log($"  → Parsing HandCardCount");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _minionStatsConfig.HandCardCount = Convert.ToInt32(token);
                GLog.Log($"  → HandCardCount set to {_minionStatsConfig.HandCardCount}");
            }
            else
            {
                GLog.Log($"  → HandCardCount skipped (empty token)");
            }
            break;
            
        case "Rarity":
            GLog.Log($"  → Parsing Rarity");
            _minionStatsConfig.Rarity = token;
            GLog.Log($"  → Rarity set to '{_minionStatsConfig.Rarity}'");
            break;
            
        case "SpellsList":
            GLog.Log($"  → Parsing SpellsList, _minionStatsConfig={((_minionStatsConfig == null) ? "NULL" : "EXISTS")}");
            if (_minionStatsConfig != null && !string.IsNullOrWhiteSpace(token))
            {
                GLog.Log($"  → Splitting SpellsList: '{token}'");
                _minionStatsConfig.SpellNames = token
                    .Split(',')
                    .Select(s => s.Trim())
                    .ToList();
                GLog.Log($"  → SpellsList set, count={_minionStatsConfig.SpellNames.Count}");
            }
            else
            {
                GLog.Log($"  → SpellsList skipped (null config or empty token)");
            }
            break;

        case "Specialization1":
        case "Specialization2":
        case "Specialization3":
        case "Specialization4":
            GLog.Log($"  → Parsing {header}");
            if (_minionStatsConfig != null && !string.IsNullOrWhiteSpace(token))
            {
                _minionStatsConfig.Specialization.Add(token);
                GLog.Log($"  → Specialization added: '{token}', total={_minionStatsConfig.Specialization.Count}");
            }
            else
            {
                GLog.Log($"  → Specialization skipped (null config or empty token)");
            }
            break;
            
        default:
            GLog.Log($"  → Unknown header: '{header}'");
            break;
    }
    
    GLog.Log($"[PARSE END] currentMinion={((_minionStatsConfig == null) ? "NULL" : $"'{_minionStatsConfig.Name}'")}");
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
                    // Пропускаем пустые имена
                    if (string.IsNullOrWhiteSpace(cfg.Name))
                    {
                        GLog.Log($"Skipping config with empty name");
                        continue;
                    }
    
                    var newSO = ScriptableObject.CreateInstance<MinionCardData>();
                    string assetPath = $"{path}/{cfg.Name}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    _targetSO.Add(so);
                    GLog.Log($"✅ Created new MinionCardData SO: {cfg.Name}");
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