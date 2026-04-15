using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.HandLogic
{
    public class HandViewSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _handContainers;

        public CardAndHealthEntityOwnerData CurrentOwner => _currentOwner;
        private readonly Dictionary<CardAndHealthEntityOwnerData, GameObject> _ownerToContainer = new();
        private CardAndHealthEntityOwnerData _currentOwner;

        public void RegisterOwner(CardAndHealthEntityOwnerData owner)
        {
            int index = _ownerToContainer.Count; 
            _ownerToContainer[owner] = _handContainers[index];
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
        }
    }
}