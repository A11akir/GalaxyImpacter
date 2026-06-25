using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.StagesGameLogic
{
    public class FightStateView : MonoBehaviour
    {
        [SerializeField] private GameObject buttonStepBack;
        [SerializeField] private Button _readyButton;

        public event Action OnReadyClicked;
        
        private void OnEnable() => _readyButton.onClick.AddListener(() => OnReadyClicked?.Invoke());
        private void OnDisable() => _readyButton.onClick.RemoveAllListeners();

        public void StartFight()
        {
            buttonStepBack.SetActive(true);
            _readyButton.interactable = true;
        }

        public void EndFight()
        {
            buttonStepBack.SetActive(false);
        }
        
        public void SetReadyButtonInteractable(bool interactable) => _readyButton.interactable = interactable;
    }
}