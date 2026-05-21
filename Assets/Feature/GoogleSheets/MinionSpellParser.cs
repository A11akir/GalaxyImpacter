#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
    public class MinionSpellParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private MinionSpellConfig _current;

        public MinionSpellParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.AllMinionSpellStats ??= new List<MinionSpellConfig>();
        }

        public void Parse(string header, string token)
        {
            switch (header)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(token)) return;
                    _current = new MinionSpellConfig { Name = token };
                    _allGameConfig.AllMinionSpellStats.Add(_current);
                    break;
                case "Cost":
                    if (_current != null && !string.IsNullOrWhiteSpace(token))
                        _current.Cost = Convert.ToInt32(token);
                    break;
                case "Value1":
                case "Value2":
                    if (_current != null && int.TryParse(token, out int val))
                        _current.Values.Add(val);
                    break;
                case "Description":
                    if (_current != null)
                        _current.Description = token;
                    break;
                case "MinionNameOwner":
                    if (_current != null)
                        _current.MinionNameOwner = token;
                    break;
                case "Type":
                    if (_current != null)
                        _current.Type = token;
                    break;
            }
        }

public void ApplyToSO()
{
    const string path = "Assets/Feature/Card/Resources/Configs";

    // Загружаем свежо прямо здесь вместо _targetSO
    var freshMinions = AssetDatabase.FindAssets("t:MinionCardData",
            new[] { "Assets/Feature/Card/Resources/Configs" })
        .Select(guid => AssetDatabase.LoadAssetAtPath<MinionCardData>(
            AssetDatabase.GUIDToAssetPath(guid)))
        .Where(m => m != null)
        .ToList();

    var allSpellSOs = new Dictionary<string, SpellCardData>();
    string[] guids = AssetDatabase.FindAssets("t:SpellCardData",
        new[] { "Assets/Feature/Card/Resources/Configs" });
    foreach (var guid in guids)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        var spellSO = AssetDatabase.LoadAssetAtPath<SpellCardData>(assetPath);
        if (spellSO != null)
            allSpellSOs[spellSO.name] = spellSO;
    }

    foreach (var cfg in _allGameConfig.AllMinionStats)
    {
        // Используем freshMinions вместо _targetSO
        var so = freshMinions.FirstOrDefault(x => x.name == cfg.Name);

        if (so == null)
        {
            if (string.IsNullOrWhiteSpace(cfg.Name))
            {
                Debug.LogWarning("Skipping config with empty name");
                continue;
            }

            var newSO = ScriptableObject.CreateInstance<MinionCardData>();
            string assetPath = $"{path}/{cfg.Name}.asset";
            AssetDatabase.CreateAsset(newSO, assetPath);
            so = newSO;
            freshMinions.Add(so);
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

        EditorUtility.SetDirty(so);
        Debug.Log($"✅ Updated MinionCardData SO: {cfg.Name}");
    }

    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
}
    }
}
#endif