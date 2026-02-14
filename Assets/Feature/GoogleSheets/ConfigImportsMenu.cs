using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Feature.GoogleSheets
{
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
            
            foreach (var sheet in itemsSheetsName)
            {
                IGoggleSheetsParser parser;
                switch (sheet)
                {
                    case "HeroStats":
                        parser = new StatsMinionParser(gameSetting);
                        break;
                    case "Cards":
                        parser = new CardsParser(gameSetting);
                        break;

                    default:
                        Debug.LogWarning($"No parser for sheet: {sheet}");
                        continue;
                }
                
                await sheetsImporter.DownloadAndParseSheet(sheet, parser);
                
                parser.ApplyToSO();
            }
        }
    }
}