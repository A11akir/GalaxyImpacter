using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace Feature.GoogleSheets
{
    public class GoogleSheetsImporter
    {
        private readonly string _sheetId;
        private readonly SheetsService _sheetsService;
        private readonly List<string> _headers = new();

        public GoogleSheetsImporter(string credentialsPath, string sheetId)
        {
            _sheetId = sheetId;

            using var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read);
            var credential = GoogleCredential.FromStream(stream)
                .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            _sheetsService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Unity Google Sheets Importer"
            });
        }

        public async Task<List<string>> GetSheetNames()
        {
            var metaRequest = _sheetsService.Spreadsheets.Get(_sheetId);
            var response = await metaRequest.ExecuteAsync();

            return response.Sheets
                .Select(s => s.Properties.Title)
                .ToList();
        }

        
        public async Task DownloadAndParseSheet(string sheetName, IGoggleSheetsParser parser)
        {
            Debug.Log($"StartingDownload sheets: {sheetName}");
            
            var range = $"{sheetName}!A:Z";
            var request = _sheetsService.Spreadsheets.Values.Get(_sheetId, range);

            ValueRange response;
            try
            {
                response = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error downloading sheet {sheetName}: {e.Message}");
                return;
            }
            
            if (response != null && response.Values != null)
            {
                var tableArray = response.Values;
                Debug.Log($"Downloaded from sheet success {sheetName}");
                
                _headers.Clear();
                
                var firstRow = tableArray[0];
                foreach (var cell in firstRow)
                {
                    _headers.Add(cell.ToString());
                }
                
                var rowsCount = tableArray.Count;
                for (int i = 1; i < rowsCount; i++)
                {
                        var row = tableArray[i];
                        var rowLength = row.Count; 
                        
                        for (int j = 0; j < rowLength; j++)
                        {
                            if (j >= _headers.Count) break;
                            
                            var cellValue = row[j];
                            var header = _headers[j];
                            
                            parser.Parse(header, cellValue.ToString());
                            
                        }
                        
                }
                
                _headers.Clear();
                Debug.Log("Sheet parsed successfully");
            }
            else
            {
                Debug.LogError($"No data found in sheet {sheetName}");
            }
        }
    }
}
