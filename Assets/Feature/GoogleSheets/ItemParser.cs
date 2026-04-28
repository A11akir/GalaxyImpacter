using Feature.Common;
using Feature.Items.Scripts;
using Unity.VisualScripting;

namespace Feature.GoogleSheets
{
#if UNITY_EDITOR
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public class ItemParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private ItemStatsConfig _itemStatsConfig;
        private readonly List<ItemData> _targetSO = new();

        public ItemParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.AllItemStats = new List<ItemStatsConfig>();
            LoadAllCardsSO();
        }

        private void LoadAllCardsSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Feature" });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (so is ItemData item)
                    _targetSO.Add(item);
            }
        }

        public void Parse(string header, string token)
        {
            switch (header)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(token))
                        return;
                    _itemStatsConfig = new ItemStatsConfig
                    {
                        ItemName = token,
                        Values = new List<int>()
                    };
                    _allGameConfig.AllItemStats.Add(_itemStatsConfig);
                    break;

                case "GoldCost":
                    if (!string.IsNullOrWhiteSpace(token))
                        _itemStatsConfig.GoldCost = Convert.ToInt32(token);
                    break;
                
                case "Description":
                    if (!string.IsNullOrWhiteSpace(token))
                        _itemStatsConfig.Description = token;
                    break;

                case "Value1":
                case "Value2":
                case "Value3":
                    if (_itemStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        if (int.TryParse(token, out int value))
                        {
                            _itemStatsConfig.Values.Add(value);
                            Debug.Log($"Добавлено значение {value} для {_itemStatsConfig.ItemName} из {header}");
                        }
                    }
                    break;

                default:
                    GLog.Log($"  → Unknown header: '{header}'");
                    break;
            }
        }

        public void ApplyToSO()
        {
            const string path = "Assets/Feature/Items/Resources/Configs";

            foreach (var cfg in _allGameConfig.AllItemStats)
            {
                var so = _targetSO.FirstOrDefault(x => (x as ScriptableObject)?.name == cfg.ItemName);

                if (so == null)
                {
                    if (string.IsNullOrWhiteSpace(cfg.ItemName))
                    {
                        GLog.Log($"Skipping config with empty name");
                        continue;
                    }

                    var newSO = ScriptableObject.CreateInstance<ItemData>();
                    string assetPath = $"{path}/{cfg.ItemName}.asset";
                    AssetDatabase.CreateAsset(newSO, assetPath);
                    so = newSO;
                    _targetSO.Add(so);
                    GLog.Log($"✅ Created new ItemCardData SO: {cfg.ItemName}");
                }

                so.ItemName = cfg.ItemName;
                so.GoldCost = cfg.GoldCost;
                so.Values = cfg.Values;
                so.Description = cfg.Description;

                EditorUtility.SetDirty(so as UnityEngine.Object);
                Debug.Log($"✅ Updated ItemCardData SO: {cfg.ItemName}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
#endif
}