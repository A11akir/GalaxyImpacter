using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Data;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
#if UNITY_EDITOR
    public class ConfigImportsMenu
    {
        private static string spreadsheetId = "1a7yYFInQjZXkeCFavpWwcT91aE90bWddaxrV4Pu-9Lw";
        private static List<string> itemsSheetsName;
        private static string credentialsPath = "galaxyimpacter-62f8e96881c2.json";

        [MenuItem("GoogleSheets/Import All Configs")]
        private static async void LoadItemsSettings()
        {
            var sheetsImporter = new GoogleSheetsImporter(credentialsPath, spreadsheetId);
            itemsSheetsName = await sheetsImporter.GetSheetNames();

            var gameSetting = new AllGameConfig();
            var parsers = new Dictionary<string, IGoggleSheetsParser>();

            foreach (var sheet in itemsSheetsName)
            {
                IGoggleSheetsParser parser = sheet switch
                {
                    "HeroStats"   => new StatsMinionParser(gameSetting),
                    "SpellStats"  => new SpellParser(gameSetting),
                    "MinionStats" => new MinionParser(gameSetting),
                    "Items"       => new ItemParser(gameSetting),
                    _             => null
                };

                if (parser == null)
                {

                    continue;
                }

                await sheetsImporter.DownloadAndParseSheet(sheet, parser);
                parsers[sheet] = parser;
            }

            var applyOrder = new[] { "HeroStats", "MinionStats", "SpellStats", "Items" };

            foreach (var sheet in applyOrder)
            {
                if (parsers.TryGetValue(sheet, out var parser))
                {
                    parser.ApplyToSO();
                    Debug.Log($"✅ Applied: {sheet}");
                }
            }
            
            var gameData = AssetDatabase.FindAssets("t:GameData")
                .Select(guid => AssetDatabase.LoadAssetAtPath<GameData>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(x => x != null);

            if (gameData == null)
            {
                Debug.LogError("GameData asset not found!");
                return;
            }

            var allCards = AssetDatabase.FindAssets("t:CardStatsData",
                    new[] { "Assets/Feature/Card/Resources/Configs" })
                .Select(guid => AssetDatabase.LoadAssetAtPath<CardStatsData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(c => c != null)
                .ToList();

            PopulateGameDataLists(gameData, allCards);
        }

        private static void PopulateGameDataLists(GameData gameData, List<CardStatsData> allCards)
        {
            gameData.baseCards.Clear();
            gameData.alchemistCards.Clear();
            gameData.assassinCards.Clear();
            gameData.earthMageCards.Clear();
            gameData.explorerCards.Clear();
            gameData.fireMageCards.Clear();
            gameData.monsterCards.Clear();
            gameData.warriorCards.Clear();
            gameData.waterMageCards.Clear();
            gameData.windMageCards.Clear();
            gameData.lightningMageCards.Clear();
            gameData.metalMageCards.Clear();
            gameData.abyssLordCards.Clear();
            gameData.timeMageCards.Clear();
            gameData.witcherCards.Clear();
            gameData.dragonbornCards.Clear();
            gameData.gravityMageCards.Clear();
            gameData.supremeAlchemistCards.Clear();
            gameData.invincibleWandererCards.Clear();
            gameData.absolutePredatorCards.Clear();
            gameData.deathKingCards.Clear();
            gameData.avatarCards.Clear();
            gameData.allCards.Clear();

            var comboMap = new Dictionary<HashSet<string>, List<CardStatsData>>(HashSet<string>.CreateSetComparer())
            {
                { new HashSet<string> { "FireMage", "WindMage" },                           gameData.lightningMageCards },
                { new HashSet<string> { "FireMage", "EarthMage" },                          gameData.metalMageCards },
                { new HashSet<string> { "WaterMage", "Explorer" },                          gameData.abyssLordCards },
                { new HashSet<string> { "WindMage", "Explorer" },                           gameData.timeMageCards },
                { new HashSet<string> { "Assassin", "Alchemist" },                          gameData.witcherCards },
                { new HashSet<string> { "Warrior", "FireMage" },                            gameData.dragonbornCards },
                { new HashSet<string> { "EarthMage", "WindMage" },                          gameData.gravityMageCards },
                { new HashSet<string> { "Alchemist", "Explorer", "Monster" },               gameData.supremeAlchemistCards },
                { new HashSet<string> { "Warrior", "Assassin", "Explorer" },                gameData.invincibleWandererCards },
                { new HashSet<string> { "Warrior", "Assassin", "Monster" },                 gameData.absolutePredatorCards },
                { new HashSet<string> { "WaterMage", "Warrior", "Monster", "Alchemist" },   gameData.deathKingCards },
                { new HashSet<string> { "WindMage", "WaterMage", "FireMage", "EarthMage" }, gameData.avatarCards },
            };

            var baseMap = new Dictionary<string, List<CardStatsData>>
            {
                { "All",        gameData.baseCards},
                { "Alchemist",  gameData.alchemistCards },
                { "Assassin",   gameData.assassinCards },
                { "EarthMage",  gameData.earthMageCards },
                { "Explorer",   gameData.explorerCards },
                { "FireMage",   gameData.fireMageCards },
                { "Monster",    gameData.monsterCards },
                { "Warrior",    gameData.warriorCards },
                { "WaterMage",  gameData.waterMageCards },
                { "WindMage",   gameData.windMageCards },
            };

            foreach (var card in allCards)
            {
                if (card == null) continue;

                gameData.allCards.Add(card);

                var specs = card.Specialization?
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                if (specs.Count == 1 && specs[0] == "All")
                {
                    gameData.baseCards.Add(card);
                    continue;
                }

                if (!card.InCollection) continue;

                if (specs.Count == 0) continue;

                if (specs.Count == 1)
                {
                    if (baseMap.TryGetValue(specs[0], out var baseList))
                        baseList.Add(card);
                    continue;
                }

                var specSet = new HashSet<string>(specs);
                bool matched = false;

                foreach (var kvp in comboMap)
                {
                    if (kvp.Key.SetEquals(specSet))
                    {
                        kvp.Value.Add(card);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                    Debug.LogWarning($"No combo class for card '{card.Name}' specs: {string.Join(", ", specs)}");
            }

            EditorUtility.SetDirty(gameData);
            AssetDatabase.SaveAssets();
            Debug.Log($"✅ GameData populated: {gameData.allCards.Count} cards total");
        }
    }
#endif
}