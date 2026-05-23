using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
#if UNITY_EDITOR
    public class SpellParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private SpellStatsConfig _spellStatsConfig;

        public SpellParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.AllSpellStats = new List<SpellStatsConfig>();
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
                        Specialization = new List<string>()
                    };
                    _allGameConfig.AllSpellStats.Add(_spellStatsConfig);
                    break;

                case "Cost":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
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

                case "MinionNameOwner":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                        _spellStatsConfig.MinionNameOwner = token;
                    break;

                case "Type":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                        _spellStatsConfig.Type = token;
                    break;

                case "Value1":
                case "Value2":
                case "Value3":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        if (int.TryParse(token, out int value))
                            _spellStatsConfig.Values.Add(value);
                    }

                    break;

                case "Specialization1":
                case "Specialization2":
                case "Specialization3":
                case "Specialization4":
                    if (_spellStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                        _spellStatsConfig.Specialization.Add(token);
                    break;
            }
        }

        public void ApplyToSO()
        {
            const string path = "Assets/Feature/Card/Resources/Configs";

            var freshSpells = AssetDatabase.FindAssets("t:SpellCardData", new[] { path })
                .Select(guid => AssetDatabase.LoadAssetAtPath<SpellCardData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(s => s != null && (UnityEngine.Object)s != null)
                .ToList();

            var freshMinions = AssetDatabase.FindAssets("t:MinionCardData", new[] { path })
                .Select(guid => AssetDatabase.LoadAssetAtPath<MinionCardData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(m => m != null && (UnityEngine.Object)m != null)
                .ToList();
            
            var expectedNames = _allGameConfig.AllSpellStats
                .Where(cfg => !string.IsNullOrWhiteSpace(cfg.Name))
                .Select(cfg => cfg.Name)
                .ToHashSet();
            
            foreach (var spell in freshSpells.ToList())
            {
                if (!expectedNames.Contains(spell.name))
                {
                    string assetPath = AssetDatabase.GetAssetPath(spell);
                    Debug.Log($"[SpellParser] Deleting outdated SO: {spell.name}");
                    AssetDatabase.DeleteAsset(assetPath);
                    freshSpells.Remove(spell);
                }
            }

            foreach (var cfg in _allGameConfig.AllSpellStats)
            {
                if (string.IsNullOrWhiteSpace(cfg.Name)) continue;

                var so = freshSpells.FirstOrDefault(x => x.name == cfg.Name);

                if (so == null)
                {
                    var newSO = ScriptableObject.CreateInstance<SpellCardData>();
                    string assetPath = $"{path}/{cfg.Name}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    freshSpells.Add(so);
                    Debug.Log($"✅ Created new SpellCardData SO: {cfg.Name}");
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Rarity = cfg.Rarity;
                so.Description = cfg.Description;
                so.Specialization = cfg.Specialization;
                so.Values = cfg.Values;
                so.Level = cfg.Level;
                so.Type = cfg.Type;

                if (!string.IsNullOrWhiteSpace(cfg.Rarity))
                {
                    so.Rarity = cfg.Rarity;
                }
                else if (!string.IsNullOrWhiteSpace(cfg.MinionNameOwner))
                {
                    var minionSO = freshMinions.FirstOrDefault(m => m.name == cfg.MinionNameOwner);
                    if (minionSO != null)
                    {
                        so.Rarity = minionSO.Rarity;
                    }
                }

                EditorUtility.SetDirty(so);

                if (!string.IsNullOrWhiteSpace(cfg.MinionNameOwner))
                {
                    var minionSO = freshMinions.FirstOrDefault(m => m.name == cfg.MinionNameOwner);

                    if (minionSO != null)
                    {
                        minionSO.SpellsList ??= new List<SpellCardData>();

                        if (!minionSO.SpellsList.Contains(so))
                        {
                            minionSO.SpellsList.Add(so);
                            EditorUtility.SetDirty(minionSO);
                            Debug.Log($"✅ Added '{cfg.Name}' to '{cfg.MinionNameOwner}' SpellsList");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ Minion '{cfg.MinionNameOwner}' not found for spell '{cfg.Name}'");
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
#endif
}