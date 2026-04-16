using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GameSessionData;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Feature.HandLogic
{
    public class HandViewSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _handContainers;

        public CardAndHealthEntityOwnerData CurrentOwner => _currentOwner;
    
        [Inject] private readonly HandCardsPositionSystem _handCardsPositionSystem;
        private readonly Dictionary<CardAndHealthEntityOwnerData, GameObject> _ownerToContainer = new();
        private readonly List<CardAndHealthEntityOwnerData> _ownerOrder = new(); 
        private CardAndHealthEntityOwnerData _currentOwner;

        public void RegisterOwner(CardAndHealthEntityOwnerData owner)
        {
            int index = _ownerToContainer.Count;
            if (index >= _handContainers.Count)
            {
                Debug.LogError($"Недостаточно контейнеров для владельца {owner}");
                return; 
            }
        
            _ownerToContainer[owner] = _handContainers[index];
            _ownerOrder.Add(owner);
            _handContainers[index].SetActive(false);
            
        }
    
        public void SwitchTo(CardAndHealthEntityOwnerData owner)
        {
            if (_currentOwner == owner) return;

            if (_currentOwner != null && _ownerToContainer.TryGetValue(_currentOwner, out var prev))
                prev.SetActive(false);

            if (_ownerToContainer.TryGetValue(owner, out var next))
                next.SetActive(true);

            _currentOwner = owner;
            
            _handCardsPositionSystem.UpdateCardsPosition();
        }
    
        [Button]
        public void SwitchToNextOwner()
        {
            if (_ownerOrder.Count < 1) return;

            int currentIndex = _ownerOrder.IndexOf(_currentOwner);
            int nextIndex = (currentIndex + 1) % _ownerOrder.Count;
            SwitchTo(_ownerOrder[nextIndex]);
        }
    }
}