using UnityEngine;

namespace Feature.GameMode
{
    public class GameModeSession : MonoBehaviour
    {
        public static GameMode GameMode { get; set; }
        
        public static void SelectOnlineMood()
        {
            GameMode = GameMode.Online;
        }      
        
        public static void SelectOfflineMood()
        {
            GameMode = GameMode.Offline;
        }
    }
    

}