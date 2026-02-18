using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
#if UNITY_EDITOR
    public class CardsParser : IGoggleSheetsParser
    {
        private readonly AllGameConfig _allGameConfig;
        private CardStatsConfig _cardStatsConfig;
        private readonly List<ICardStatsData> _targetSO = new();

        public CardsParser(AllGameConfig allGameConfig)
        {
            _allGameConfig = allGameConfig;
            _allGameConfig.AllCards = new List<CardStatsConfig>();
            LoadAllCardsSO();
        }

        private void LoadAllCardsSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Feature" });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (so is ICardStatsData card)
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
                    _cardStatsConfig = new CardStatsConfig
                    {
                        Name = token,
                        Values = new List<int>(),
                        Specialization = new List<string>()
                    };
                    _allGameConfig.AllCards.Add(_cardStatsConfig);
                    break;
                    
                case "Cost":
                    if (_cardStatsConfig != null)
                        _cardStatsConfig.Cost = Convert.ToInt32(token);
                    break;              
                case "Health":
                    if (_cardStatsConfig != null)
                        _cardStatsConfig.Health = Convert.ToInt32(token);
                    break;
                    
                case "Rarity":
                    if (_cardStatsConfig != null)
                        _cardStatsConfig.Rarity = token;
                    break;
                    
                case "Description":
                    if (_cardStatsConfig != null)
                        _cardStatsConfig.Description = token;
                    break;

                case "Value1":
                case "Value2":
                case "Value3":
                    if (_cardStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        if (int.TryParse(token, out int value))
                        {
                            _cardStatsConfig.Values.Add(value);
                            Debug.Log($"Добавлено значение {value} для {_cardStatsConfig.Name} из {header}");
                        }
                    }
                    break;

                case "Specialization1":
                case "Specialization2":
                case "Specialization3":
                case "Specialization4":
                    if (_cardStatsConfig != null && !string.IsNullOrWhiteSpace(token))
                    {
                        _cardStatsConfig.Specialization.Add(token);
                        Debug.Log($"Добавлена специализация {token} для {_cardStatsConfig.Name} из {header}");
                    }
                    break;
            }
        }

        public void ApplyToSO()
        {
            foreach (var cfg in _allGameConfig.AllCards)
            {
                var so = _targetSO
                    .FirstOrDefault(x => (x as ScriptableObject).name == cfg.Name);

                if (so == null)
                {
                    Debug.LogWarning($"SO not found for card: {cfg.Name}");
                    continue;
                }

                so.Name = cfg.Name;
                so.Cost = cfg.Cost;
                so.Rarity = cfg.Rarity;
                so.Description = cfg.Description;
                so.Specialization = cfg.Specialization;
                so.Values = cfg.Values;
                so.Level = cfg.Level;
                so.Health = cfg.Health;

                EditorUtility.SetDirty(so as UnityEngine.Object);
                
                Debug.Log($"✅ Updated Card SO: {cfg.Name} - " +
                         $"Values: [{string.Join(", ", cfg.Values)}], " +
                         $"Specializations: [{string.Join(", ", cfg.Specialization)}]");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    #endif
}