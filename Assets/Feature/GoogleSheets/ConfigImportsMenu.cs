using System.Collections.Generic;
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
                    Debug.LogWarning($"No parser for sheet: {sheet}");
                    continue;
                }

                await sheetsImporter.DownloadAndParseSheet(sheet, parser);
                parsers[sheet] = parser;
            }

            var applyOrder = new[] { "SpellStats", "HeroStats", "MinionStats" , "Items" };

            foreach (var sheet in applyOrder)
            {
                if (parsers.TryGetValue(sheet, out var parser))
                {
                    parser.ApplyToSO();
                    Debug.Log($"✅ Applied: {sheet}");
                }
            }
        }
    }
    
    #endif
}