using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Feature.HandLogic
{
    public class HandViewSwitcher : MonoBehaviour
    {
        [SerializeField] private List<CardsOwnerContainer> _containers; // 7 штук в инспекторе

        [Inject] private HandCardsPositionSystem _handCardsPositionSystem;
        private readonly List<CardAndHealthEntityOwnerData> _ownerOrder = new();
        private readonly Dictionary<CardAndHealthEntityOwnerData, CardsOwnerContainer> _ownerToContainer = new();
        private CardAndHealthEntityOwnerData _currentOwner;

        public CardAndHealthEntityOwnerData CurrentOwner => _currentOwner;
        public event Action<CardAndHealthEntityOwnerData> OnOwnerSwitched;

        private void Start()
        {
            foreach (var container in _containers)
                container.gameObject.SetActive(false);
        }

        public CardsOwnerContainer RegisterOwner(CardAndHealthEntityOwnerData owner)
        {
            int index = _ownerOrder.Count;
            if (index >= _containers.Count)
            {
                Debug.LogError("Недостаточно контейнеров!");
                return null;
            }

            var container = _containers[index];
            _ownerToContainer[owner] = container;
            _ownerOrder.Add(owner);
            container.gameObject.SetActive(false);
            return container;
        }

        public CardsOwnerContainer GetContainer(CardAndHealthEntityOwnerData owner)
            => _ownerToContainer.TryGetValue(owner, out var container) ? container : null;
    
        public void SwitchTo(CardAndHealthEntityOwnerData owner)
        {
            if (_currentOwner == owner) return;

            if (_currentOwner != null && _ownerToContainer.TryGetValue(_currentOwner, out var prev))
                prev.gameObject.SetActive(false);

            if (_ownerToContainer.TryGetValue(owner, out var next))
                next.gameObject.SetActive(true);

            _currentOwner = owner;
            OnOwnerSwitched?.Invoke(owner);
        
            _handCardsPositionSystem.UpdateCardsPosition(_ownerToContainer[owner].HandCardsContainer);
        }

        [Button]
        public void SwitchToNextOwner()
        {
            if (_ownerOrder.Count < 1) return;
            int nextIndex = (_ownerOrder.IndexOf(_currentOwner) + 1) % _ownerOrder.Count;
            SwitchTo(_ownerOrder[nextIndex]);
        }
    }
}