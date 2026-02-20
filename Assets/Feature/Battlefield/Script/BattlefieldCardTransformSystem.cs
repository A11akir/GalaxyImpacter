using Sirenix.OdinInspector;
using UnityEngine;

namespace Feature.Battlefield.Script
{
    public class BattlefieldCardTransformSystem : MonoBehaviour
    {
        [SerializeField] private Transform battlefieldParent;
        [SerializeField] private float horizontalSpacing = 100f;
        
        [Button]
        public void UpdateCardsPosition()
        {
            int activeCardsCount = 0;
            
            foreach (Transform child in battlefieldParent)
                if (child.gameObject.activeInHierarchy)
                    activeCardsCount++;

            if (activeCardsCount == 0) return;

            float totalWidth = (activeCardsCount - 1) * horizontalSpacing;
            float startX = -totalWidth / 2f;
            int currentIndex = 0;
            
            foreach (Transform child in battlefieldParent)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    float xPos = startX + (currentIndex * horizontalSpacing);
                    child.localPosition = new Vector3(xPos, 0, 0);
                    currentIndex++;
                }
            }
        }
    }
}