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

            var freshSpells = AssetDatabase.FindAssets("t:SpellCardData", new[] { path })
                .Select(guid => AssetDatabase.LoadAssetAtPath<SpellCardData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(s => s != null)
                .ToList();

            foreach (var cfg in _allGameConfig.AllMinionSpellStats)
            {
                if (string.IsNullOrWhiteSpace(cfg.Name))
                {
                    continue;
                }

                var so = freshSpells.FirstOrDefault(x => x.name == cfg.Name);

                if (so == null)
                {
                    var newSO = ScriptableObject.CreateInstance<SpellCardData>();
                    string assetPath = $"{path}/{cfg.Name}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    freshSpells.Add(so);
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Values = cfg.Values;
                so.Description = cfg.Description;
                so.MinionNameOwner = cfg.MinionNameOwner;
                so.Type = cfg.Type;

                EditorUtility.SetDirty(so);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif