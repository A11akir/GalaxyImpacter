using System.Collections.Generic;
using System.Linq;
using Feature.Card.Script;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Feature.HandLogic
{
    public class HandCardsPositionSystem : MonoBehaviour
    {
        [Inject] private HandCardViews _handCardViews;
        
        private readonly List<GameObject> _handCards = new List<GameObject>();
        
        [SerializeField] private Transform handParent;
        [SerializeField] private int verticalCardOffset = 30; 
        [SerializeField] private int verticalCardOffsetRatio = 5;  
        [SerializeField] private int cardOffsetRotate = 15;
        [SerializeField] private float cardOffsetRotateRatio = 1;        
        [SerializeField] private int horizontalCardOffset = 150; 
        [SerializeField] private int horizontalCardOffsetRatio = 10;


        public void OnEnable()
        {
            _handCardViews.UpdateViewCard += UpdateCardsPosition;
        }
        public void OnDisable()
        {
            _handCardViews.UpdateViewCard -= UpdateCardsPosition;
        }
        
        [Button]
        public void UpdateCardsPosition()
        {
            CollectHandCards();

            for (int i = 0; i < _handCards.Count; i++)
            {
                var rotateZ = CalculateRotateZ(i);
                var yPos = CalculateYPos(i);
                var xPos = CalculateXPos(i);

                _handCards[i].transform.localScale = Vector3.one;
                _handCards[i].transform.localPosition = new Vector3(xPos, yPos, 0);
                if (!float.IsNaN(rotateZ)) _handCards[i].transform.localRotation = Quaternion.Euler(0, 0, rotateZ);
            }
        }

        private float CalculateRotateZ(int i)
        {
            if (_handCards.Count == 1) return 0;
            
            float centerIndex = (_handCards.Count - 1) / 2f;
            
            float rotateZ = (i - centerIndex) / centerIndex * (-cardOffsetRotate - cardOffsetRotateRatio * Mathf.Min(i, _handCards.Count - 1 - i));
    
            return rotateZ;
        }

        private float CalculateXPos(int i)
        {
            float spacing = horizontalCardOffset
                            - (_handCards.Count - 1) * horizontalCardOffsetRatio;

            spacing = Mathf.Max(spacing, 20f);
            
            return (i - (_handCards.Count - 1) / 2f) * spacing;
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
                if (child.gameObject.activeInHierarchy)
                    _handCards.Add(child.gameObject);
        }
    }
}