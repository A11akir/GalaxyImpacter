using TMPro;
using UnityEngine;

namespace Feature.EndGameSession
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject gameEndScreen;
        [SerializeField] private TextMeshProUGUI gameOverTextPlayers;
        public void ShowVictory()
        {
            gameEndScreen.SetActive(true);
            gameOverTextPlayers.text = "You Win!";
        }

        public void ShowDefeat()
        {
            gameEndScreen.SetActive(true);
            gameOverTextPlayers.text = "You Lose!";
        }
    }
}