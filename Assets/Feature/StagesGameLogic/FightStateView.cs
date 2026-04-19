using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.StagesGameLogic
{
    public class FightStateView : MonoBehaviour
    {
        [SerializeField] private GameObject fightPhone;
        [SerializeField] private GameObject buttonStepBack;
        [SerializeField] private Button _readyButton;

        public event Action OnReadyClicked;
        
        private void OnEnable() => _readyButton.onClick.AddListener(() => OnReadyClicked?.Invoke());
        private void OnDisable() => _readyButton.onClick.RemoveAllListeners();

        public void StartFight()
        {
            fightPhone.SetActive(true);
            buttonStepBack.SetActive(true);
        }

        public void EndFight()
        {
            fightPhone.SetActive(false);
            buttonStepBack.SetActive(false);
        }
    }
}