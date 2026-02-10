using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Feature.HandLogic
{
    public class HandCardsPositionSystem : MonoBehaviour
    {
        private readonly List<GameObject> _handCards = new List<GameObject>();
        
        [SerializeField] private Transform handParent;
        [SerializeField] private int verticalCardOffset = 50;        
        [SerializeField] private int horizontalCardOffset = 100;

        [Button]
        private void UpdateCardsPosition()
        {
            CollectHandCards();

            for (int i = 0; i < _handCards.Count; i++)
            {
                float yPos = verticalCardOffset * Mathf.Min(i, _handCards.Count - 1 - i);
                
                float centerOffset = (_handCards.Count - 1) * horizontalCardOffset / 2f;
                float xPos = (i * horizontalCardOffset) - centerOffset;
                
                _handCards[i].transform.localPosition = new Vector3(xPos, yPos, 0);
            }
        }

        private void CollectHandCards()
        {
            _handCards.Clear();
            
            foreach (Transform child in handParent)
                _handCards.Add(child.gameObject);
        }
    }
}