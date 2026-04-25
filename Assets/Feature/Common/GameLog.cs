using System.IO;
using UnityEngine;

namespace Feature.Common
{
    public static class GLog
    {
        private static string _path = Application.dataPath + "/../game_log.txt";
    
        public static void Log( string message)
        {
            var line = $"{message}";
            Debug.Log(line);
            File.AppendAllText(_path, line + "\n");
        }
    
        public static void Clear()
        {
            File.WriteAllText(_path, "");
        }
    }
}