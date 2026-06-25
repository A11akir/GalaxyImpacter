using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.StagesGameLogic
{
    public class PrepareStateView : MonoBehaviour
    {
        [SerializeField] private GameObject buttonFight;
        [SerializeField] private Button _readyButton;

        public event Action OnReadyClicked;

        private void OnEnable() => _readyButton.onClick.AddListener(() => OnReadyClicked?.Invoke());
        private void OnDisable() => _readyButton.onClick.RemoveAllListeners();
        
        public void StartPrepare()
        {
            buttonFight.SetActive(true);
            _readyButton.interactable = true;
        }

        public void EndPrepare()
        {
            buttonFight.SetActive(false);
        }
        
        public void SetReadyButtonInteractable(bool interactable) => _readyButton.interactable = interactable;
    }
}