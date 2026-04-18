using Feature.Card.Script;
using Feature.Chakra;
using UnityEngine;

namespace Feature.HandLogic
{
    public class CardsOwnerContainer : MonoBehaviour
    {
        [SerializeField] private HandCardViews _handCardViews;
        [SerializeField] private Transform _handCardsContainer;
        [SerializeField] private ChakraWindowView _chakraWindowView;

        public HandCardViews HandCardViews => _handCardViews;
        public Transform HandCardsContainer => _handCardsContainer;
        public ChakraWindowView ChakraWindowView => _chakraWindowView;
    }
}