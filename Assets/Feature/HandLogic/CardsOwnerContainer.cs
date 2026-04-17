using Feature.Card.Script;
using Feature.Chakra;
using UnityEngine;

namespace Feature.HandLogic
{
    public class CardsOwnerContainer : MonoBehaviour
    {
        [SerializeField] private HandCardViews _handCardViews;
        [SerializeField] private HandCardsPositionSystem _handCardsPositionSystem;
        [SerializeField] private ChakraWindowView _chakraWindowView;

        public HandCardViews HandCardViews => _handCardViews;
        public HandCardsPositionSystem HandCardsPositionSystem => _handCardsPositionSystem;
        public ChakraWindowView ChakraWindowView => _chakraWindowView;
    }
}