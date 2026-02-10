using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Feature.HandLogic
{
    public class HandCardsPositionSystem : MonoBehaviour
    {
        private readonly List<GameObject> _handCards = new List<GameObject>();
        
        [SerializeField] private Transform handParent;
        [SerializeField] private int verticalCardOffset = 30; 
        [SerializeField] private int verticalCardOffsetRatio = 5;  
        [SerializeField] private int cardOffsetRotate = 15;
        [SerializeField] private float cardOffsetRotateRatio = 1;        
        [SerializeField] private int horizontalCardOffset = 150; 
        [SerializeField] private int horizontalCardOffsetRatio = 10;

        [Button]
        private void UpdateCardsPosition()
        {
            CollectHandCards();

            for (int i = 0; i < _handCards.Count; i++)
            {
                float centerOffset = (_handCards.Count - 1) * horizontalCardOffset / 2f;
                Debug.Log(centerOffset);
                
                var rotateZ = CalculateRotateZ(i);
                var yPos = CalculateYPos(i);
                var xPos = CalculateXPos(i);
                
                _handCards[i].transform.localPosition = new Vector3(xPos, yPos, 0);
                if (!float.IsNaN(rotateZ)) _handCards[i].transform.localRotation = Quaternion.Euler(0, 0, rotateZ);
            }
        }

        private float CalculateRotateZ(int i)
        {
            if (_handCards.Count == 1) return 0;
            
            int distanceFromEdge = Mathf.Min(i, _handCards.Count - 1 - i);
    
            float centerIndex = (_handCards.Count - 1) / 2f;
            float offsetFromCenter = i - centerIndex;
    
            float normalizedOffset = offsetFromCenter / centerIndex;
    
            float rotateZ = normalizedOffset * (-cardOffsetRotate - cardOffsetRotateRatio * distanceFromEdge);
    
            return rotateZ;
        }

        private float CalculateXPos(int i)
        {
            int count = _handCards.Count;

            float spacing = horizontalCardOffset
                            - (count - 1) * horizontalCardOffsetRatio;

            spacing = Mathf.Max(spacing, 20f);

            float centerIndex = (count - 1) / 2f;

            return (i - centerIndex) * spacing;
        }

        private float CalculateYPos(int i)
        {
            float yPos = (verticalCardOffset - verticalCardOffsetRatio*_handCards.Count)
                         * Mathf.Min(i, _handCards.Count - 1 - i);
            return yPos;
        }

        private void CollectHandCards()
        {
            _handCards.Clear();
            
            foreach (Transform child in handParent)
                _handCards.Add(child.gameObject);
        }
    }
}