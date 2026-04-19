using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.StagesGameLogic
{
    public class PrepareStateView : MonoBehaviour
    {
        [SerializeField] private GameObject preparePhone;
        [SerializeField] private GameObject buttonFight;
        [SerializeField] private Button _readyButton;

        public event Action OnReadyClicked;

        private void OnEnable() => _readyButton.onClick.AddListener(() => OnReadyClicked?.Invoke());
        private void OnDisable() => _readyButton.onClick.RemoveAllListeners();
        
        public void StartPrepare()
        {
            preparePhone.SetActive(true);
            buttonFight.SetActive(true);
        }

        public void EndPrepare()
        {
            preparePhone.SetActive(false);  
            buttonFight.SetActive(false);
        }
    }
}