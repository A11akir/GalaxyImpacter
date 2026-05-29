using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Feature.Data;
using Feature.Hero;
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

            var comboMap = new Dictionary<HashSet<AllHeroClass>, List<CardStatsData>>(HashSet<AllHeroClass>.CreateSetComparer())
            {
                { new HashSet<AllHeroClass> { AllHeroClass.FireMage,  AllHeroClass.WindMage },                                                      gameData.lightningMageCards },
                { new HashSet<AllHeroClass> { AllHeroClass.FireMage,  AllHeroClass.EarthMage },                                                     gameData.metalMageCards },
                { new HashSet<AllHeroClass> { AllHeroClass.WaterMage, AllHeroClass.Explorer },                                                      gameData.abyssLordCards },
                { new HashSet<AllHeroClass> { AllHeroClass.WindMage,  AllHeroClass.Explorer },                                                      gameData.timeMageCards },
                { new HashSet<AllHeroClass> { AllHeroClass.Assassin,  AllHeroClass.Alchemist },                                                     gameData.witcherCards },
                { new HashSet<AllHeroClass> { AllHeroClass.Warrior,   AllHeroClass.FireMage },                                                      gameData.dragonbornCards },
                { new HashSet<AllHeroClass> { AllHeroClass.EarthMage, AllHeroClass.WindMage },                                                      gameData.gravityMageCards },
                { new HashSet<AllHeroClass> { AllHeroClass.Alchemist, AllHeroClass.Explorer,  AllHeroClass.Monster },                               gameData.supremeAlchemistCards },
                { new HashSet<AllHeroClass> { AllHeroClass.Warrior,   AllHeroClass.Assassin,  AllHeroClass.Explorer },                              gameData.invincibleWandererCards },
                { new HashSet<AllHeroClass> { AllHeroClass.Warrior,   AllHeroClass.Assassin,  AllHeroClass.Monster },                               gameData.absolutePredatorCards },
                { new HashSet<AllHeroClass> { AllHeroClass.WaterMage, AllHeroClass.Warrior,   AllHeroClass.Monster,  AllHeroClass.Alchemist },       gameData.deathKingCards },
                { new HashSet<AllHeroClass> { AllHeroClass.WindMage,  AllHeroClass.WaterMage, AllHeroClass.FireMage, AllHeroClass.EarthMage },       gameData.avatarCards },
            };

            var baseMap = new Dictionary<AllHeroClass, List<CardStatsData>>
            {
                { AllHeroClass.All,       gameData.baseCards },
                { AllHeroClass.Alchemist, gameData.alchemistCards },
                { AllHeroClass.Assassin,  gameData.assassinCards },
                { AllHeroClass.EarthMage, gameData.earthMageCards },
                { AllHeroClass.Explorer,  gameData.explorerCards },
                { AllHeroClass.FireMage,  gameData.fireMageCards },
                { AllHeroClass.Monster,   gameData.monsterCards },
                { AllHeroClass.Warrior,   gameData.warriorCards },
                { AllHeroClass.WaterMage, gameData.waterMageCards },
                { AllHeroClass.WindMage,  gameData.windMageCards },
            };

            foreach (var card in allCards)
            {
                if (card == null) continue;

                gameData.allCards.Add(card);

                var specs = card.Specialization ?? new List<AllHeroClass>();

                if (specs.Count == 1 && specs[0] == AllHeroClass.All)
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

                var specSet = new HashSet<AllHeroClass>(specs);
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