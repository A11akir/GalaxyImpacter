using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Feature.Card.Script
{
    public class HandCardViews : MonoBehaviour
    {
        [FormerlySerializedAs("_cardsInDeck")] [SerializeField] public List<HandCardView> _cardsInHand;

        public event Action UpdateViewCard;

        
        public HandCardView AddCardFromHand(CardStatsData cardStatsData, int addedIndex)
        {
            Transform handContainer = transform;
            
            HandCardView freeView = null;
            for (int i = 0; i < handContainer.childCount; i++)
            {
                var child = handContainer.GetChild(i).GetComponent<HandCardView>();
                if (!child.gameObject.activeSelf)
                {
                    freeView = child;
                    break;
                }
            }
    
            if (freeView == null) return null;
    
            freeView.SetDataView(cardStatsData);
            freeView.transform.SetSiblingIndex(addedIndex);
            return freeView;
        }

        public HandCardView AddCardAsHiddenForEnemyPlayer(int addedIndex)
        {
            Transform handContainer = transform;
    
            HandCardView freeView = null;
            for (int i = handContainer.childCount - 1; i >= 0; i--)
            {
                var child = handContainer.GetChild(i).GetComponent<HandCardView>();
                if (!child.gameObject.activeSelf)
                {
                    freeView = child;
                    break;
                }
            }
    
            if (freeView == null) return null;
    
            freeView.ShowAsHidden();
            freeView.transform.SetSiblingIndex(addedIndex);
            return freeView;
        }

        public void RemoveHandCardView(HandCardView view)
        {
            view.ClearData();
            view.transform.SetAsLastSibling();
            view.gameObject.SetActive(false);
        }
    }
}