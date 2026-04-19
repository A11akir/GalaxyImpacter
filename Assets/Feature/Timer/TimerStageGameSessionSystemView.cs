// TimerStageGameSessionSystemView.cs
using TMPro;
using UnityEngine;

namespace Feature.Timer
{
    public class TimerStageGameSessionSystemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        public void SetTime(float timeLeft)
        {
            int seconds = Mathf.CeilToInt(timeLeft);
            _timerText.text = seconds.ToString();
        }
    }
}