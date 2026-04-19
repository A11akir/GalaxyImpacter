using Feature.Timer;
using UnityEngine;
using Zenject;

namespace Feature.Common
{
    public class UpdateSystem : MonoBehaviour
    {
        [Inject] private TimerSystem _timerSystem;
        [Inject] private TimerStageGameSessionPresenter _timerPresenter;

        private void Update()
        {
            _timerSystem.Tick(Time.deltaTime);
            _timerPresenter.Tick();
        }
    }
}