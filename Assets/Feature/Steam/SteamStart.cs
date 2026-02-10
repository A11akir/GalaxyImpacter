using Steamworks;
using UnityEngine;

namespace Feature.Steam
{
    public class SteamStart : MonoBehaviour
    {
        public void InitSteam()
        {
            if (SteamManager.Initialized)
            {
                string name = SteamFriends.GetPersonaName();
                Debug.Log(name);
            }
        }
    }
}