using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Feature.Battlefield.Script
{
    public class BattlefieldCardTransformSystem : MonoBehaviour
    {
        [SerializeField] private Transform battlefieldParent;
        [SerializeField] private float horizontalSpacing = 100f;
        
        private readonly List<GameObject> _battlefieldCards = new List<GameObject>();
        
        [Button]
        public void UpdateCardsPosition()
        {
            CollectHandCards();

            float totalWidth = (_battlefieldCards.Count - 1) * horizontalSpacing;
            float startX = -totalWidth / 2f;

            for (int i = 0; i < _battlefieldCards.Count; i++)
            {
                float xPos = startX + (i * horizontalSpacing);
                _battlefieldCards[i].transform.localPosition = new Vector3(xPos, 0, 0);
            }
        }

        private void CollectHandCards()
        {
            _battlefieldCards.Clear();
            
            foreach (Transform child in battlefieldParent)
                if (child.gameObject.activeInHierarchy)
                    _battlefieldCards.Add(child.gameObject);
        }
    }
}