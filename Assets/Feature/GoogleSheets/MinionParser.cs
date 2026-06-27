#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feature.Card.Script;
using Feature.Common;
using Feature.Hero;
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
                    if (string.IsNullOrWhiteSpace(token))
                        return;
                    _minionStatsConfig = new MinionStatsConfig
                    {
                        Name = token,
                        Values = new List<int>(),
                        Specialization = new List<AllHeroClass>()
                    };
                    _allGameConfig.AllMinionStats.Add(_minionStatsConfig);
                    break;
                case "Cost":
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _minionStatsConfig.Cost = Convert.ToInt32(token);
                    }
                    break;
                case "Health":
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _minionStatsConfig.Health = Convert.ToInt32(token);
                    }
                    break;
                case "Chakra":
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _minionStatsConfig.Chakra = Convert.ToInt32(token);
                    }
                    break;
                case "HandCardCount":
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _minionStatsConfig.HandCardCount = Convert.ToInt32(token);
                    }
                    break;
                case "Rarity":
                    _minionStatsConfig.Rarity = token;
                    break;
                case "SpellsList":
                    if (_minionStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        _minionStatsConfig.SpellNames = token
                            .Split(',')
                            .Select(s => s.Trim())
                            .ToList();
                    }

                    break;
                case "Specialization1":
                case "Specialization2":
                case "Specialization3":
                case "Specialization4":
                    if (_minionStatsConfig != null && !string.IsNullOrWhiteSpace(token) &&
                        Enum.TryParse<AllHeroClass>(token, out var minionClass))
                        _minionStatsConfig.Specialization.Add(minionClass);
                    break;
                case "InCollection":
                    if (_minionStatsConfig != null)
                        _minionStatsConfig.InCollection = token == "TRUE" || token == "1" || token.ToLower() == "true";
                    break;
            }
        }

        public void ApplyToSO()
        {
            const string path = "Assets/Feature/Card/Resources/Configs/Minions";

            var allSpellSOs = new Dictionary<string, SpellCardData>();
            string[] guids =
                AssetDatabase.FindAssets("t:SpellCardData", new[] { "Assets/Feature/Card/Resources/Configs" });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var spellSO = AssetDatabase.LoadAssetAtPath<SpellCardData>(assetPath);
                if (spellSO != null)
                    allSpellSOs[spellSO.name] = spellSO;
            }

            var baseSpells = allSpellSOs.Values
                .Where(s => s.Specialization != null
                            && s.Specialization.Count == 1
                            && s.Specialization[0] == AllHeroClass.All)
                .ToList();

            _targetSO.RemoveAll(x => x == null || (x as UnityEngine.Object) == null);

            foreach (var cfg in _allGameConfig.AllMinionStats)
            {
                var so = _targetSO.FirstOrDefault(x =>
                {
                    var obj = x as ScriptableObject;
                    return obj != null && obj.name == cfg.Name;
                });

                if (so == null)
                {
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

                    EnsureSpellsFolderForMinion(cfg.Name);
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Rarity = CardRarityConverter.FromString(cfg.Rarity);
                so.Specialization = cfg.Specialization;
                so.Level = cfg.Level;
                so.Health = cfg.Health;
                so.Chakra = cfg.Chakra;
                so.HandCardCount = cfg.HandCardCount;
                so.InCollection = cfg.InCollection;
                so.TargetType = cfg.TargetType;
                so.BaseCost = cfg.Cost; 

                bool isBaseMinion = cfg.Specialization != null
                                    && cfg.Specialization.Count == 1
                                    && cfg.Specialization[0] == AllHeroClass.All;

                so.SpellsList = isBaseMinion
                    ? new List<SpellCardData>(baseSpells)
                    : new List<SpellCardData>();

                EditorUtility.SetDirty(so as UnityEngine.Object);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void EnsureSpellsFolderForMinion(string minionName)
        {
            const string spellsPath = "Assets/Feature/Card/Resources/Configs/Spells";
            string fullPath = Path.Combine(
                Application.dataPath,
                $"{spellsPath.Substring("Assets/".Length)}/{minionName}");

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }
    }
}
#endif