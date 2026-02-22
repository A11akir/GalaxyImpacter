using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

namespace Feature.Battlefield.Script
{
    public class BattlefieldCardTransformSystem : MonoBehaviour
    {
        [SerializeField] private Transform battlefieldParent;
        [SerializeField] private float horizontalSpacing = 100f;
        [SerializeField] private float horizontalSpacingCoef = 7f;

        [Button]
        public void UpdateCardsPosition(Transform transformCard)
        {
            
        }
    }
}