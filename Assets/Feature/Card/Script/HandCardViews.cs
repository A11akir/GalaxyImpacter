using System;
using System.Collections.Generic;
using Feature.Hero;
using R3;
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
            HandCardView lastView = handContainer.GetChild(handContainer.childCount - 1).GetComponent<HandCardView>();
            lastView.SetDataView(cardStatsData);
            lastView.transform.SetSiblingIndex(addedIndex);
            UpdateViewCard?.Invoke();
            return lastView;
        }
        
        public void RemoveHandCardView(HandCardView view)
        {
            view.ClearData();
            view.transform.SetAsLastSibling();
            view.gameObject.SetActive(false);
        }
    }
}