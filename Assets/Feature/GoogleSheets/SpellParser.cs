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

                if (so is ISpellStatsData spell)
                    _targetSO.Add(spell);
            }
        }

        public void Parse(string header, string token)
        {
            switch (header)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(token)) return;
                    _spellStatsConfig = new SpellStatsConfig
                    {
                        Name = token,
                        Values = new List<int>(),
                        Specialization = new List<AllHeroClass>()
                    };
                    _allGameConfig.AllSpellStats.Add(_spellStatsConfig);
                    break;

                case "Cost":
                    if (!string.IsNullOrWhiteSpace(token))
                        _spellStatsConfig.Cost = Convert.ToInt32(token);
                    break;

                case "Rarity":
                    if (!string.IsNullOrWhiteSpace(token))
                        _spellStatsConfig.Rarity = token;
                    break;

                case "Description":
                    _spellStatsConfig.Description = token;
                    break;

                case "MinionNameOwner":
                    if (!string.IsNullOrWhiteSpace(token))
                        _spellStatsConfig.MinionNameOwner = token;
                    break;

                case "Type":
                    if (!string.IsNullOrWhiteSpace(token))
                        if (Enum.TryParse<TargetType>(token, out var targetType))
                            _spellStatsConfig.TargetType = targetType;
                    break;

                case "Value1":
                case "Value2":
                case "Value3":
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        if (int.TryParse(token, out int value))
                            _spellStatsConfig.Values.Add(value);
                    }
                    break;

                case "Specialization1":
                case "Specialization2":
                case "Specialization3":
                case "Specialization4":
                    if (!string.IsNullOrWhiteSpace(token) && Enum.TryParse<AllHeroClass>(token, out var spellClass))
                        _spellStatsConfig.Specialization.Add(spellClass);
                    break;
                case "InCollection":
                    if (_spellStatsConfig != null)
                        _spellStatsConfig.InCollection = token == "TRUE" || token == "1" || token.ToLower() == "true";
                    break;
            }
        }

        public void ApplyToSO()
        {
            const string spellsPath = "Assets/Feature/Card/Resources/Configs/Spells";
            const string minionsPath = "Assets/Feature/Card/Resources/Configs/Minions";

            var allMinionSOs = new Dictionary<string, MinionCardData>();
            string[] minionGuids = AssetDatabase.FindAssets("t:MinionCardData", new[] { minionsPath });

            foreach (var guid in minionGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var minionSO = AssetDatabase.LoadAssetAtPath<MinionCardData>(assetPath);
                if (minionSO != null)
                    allMinionSOs[minionSO.name] = minionSO;
            }
            

            _targetSO.RemoveAll(x => x == null || (x as UnityEngine.Object) == null);

            var processedNames = new HashSet<string>();

            foreach (var cfg in _allGameConfig.AllSpellStats)
            {
                if (string.IsNullOrWhiteSpace(cfg.Name)) continue;

                if (!processedNames.Add(cfg.Name))
                {
                    continue;
                }
                

                var so = _targetSO.FirstOrDefault(x =>
                {
                    var obj = x as ScriptableObject;
                    return obj != null && obj.name == cfg.Name;
                });

                if (so == null)
                {
                    var newSO = ScriptableObject.CreateInstance<SpellCardData>();
                    string folder = GetSpellFolder(cfg, spellsPath);
                    EnsureFolderExists(folder);
                    string assetPath = $"{folder}/{cfg.Name}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    _targetSO.Add(so);
                }
                
                if (so == null || (so as UnityEngine.Object) == null)
                {
                    GLog.Log($"[SpellParser] Failed to create SO for: '{cfg.Name}'");
                    continue;
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Description = cfg.Description;
                so.Specialization = cfg.Specialization;
                so.Values = cfg.Values;
                so.Level = cfg.Level;
                so.TargetType = cfg.TargetType;
                so.InCollection = cfg.InCollection;
                so.BaseCost = cfg.Cost; 

                if (!string.IsNullOrWhiteSpace(cfg.Rarity))
                    so.Rarity = CardRarityConverter.FromString(cfg.Rarity);
                else if (!string.IsNullOrWhiteSpace(cfg.MinionNameOwner) &&
                         allMinionSOs.TryGetValue(cfg.MinionNameOwner, out var owner))
                    so.Rarity = owner.Rarity;

                if (!string.IsNullOrWhiteSpace(cfg.MinionNameOwner))
                {
                    if (allMinionSOs.TryGetValue(cfg.MinionNameOwner, out var minionSO) 
                        && minionSO != null 
                        && minionSO != null)
                    {
                        minionSO.SpellsList ??= new List<SpellCardData>();

                        if (!minionSO.SpellsList.Contains(so as SpellCardData))
                        {
                            minionSO.SpellsList.Add(so as SpellCardData);
                            EditorUtility.SetDirty(minionSO);
                        }
                    }
                    else
                    {
                        GLog.Log($"[SpellParser] Minion '{cfg.MinionNameOwner}' not found or destroyed for spell '{cfg.Name}'");
                    }
                }

                EditorUtility.SetDirty(so as UnityEngine.Object);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string GetSpellFolder(SpellStatsConfig cfg, string spellsPath)
        {
            if (!string.IsNullOrWhiteSpace(cfg.MinionNameOwner))
                return $"{spellsPath}/{cfg.MinionNameOwner}";

            if (cfg.Specialization == null || cfg.Specialization.Count == 0)
                return spellsPath;

            if (cfg.Specialization.Count == 1)
                return $"{spellsPath}/{cfg.Specialization[0]}";

            var combo = FindComboClass(cfg.Specialization);
            return combo.HasValue
                ? $"{spellsPath}/{combo.Value}"
                : $"{spellsPath}/{cfg.Specialization[0]}";
        }

        private ComboHeroClass? FindComboClass(List<AllHeroClass> specializations)
        {
            foreach (var kvp in ComboClassRequirements.Requirements)
            {
                var required = kvp.Value;
                if (required.Count != specializations.Count) continue;

                bool matches = required.All(baseClass =>
                    specializations.Contains(ComboClassRequirements.ToAllHeroClass(baseClass)));

                if (matches) return kvp.Key;
            }
            return null;
        }

        private void EnsureFolderExists(string assetFolderPath)
        {
            string fullPath = Path.Combine(Application.dataPath, assetFolderPath.Substring("Assets/".Length));
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }
    }
}
#endif