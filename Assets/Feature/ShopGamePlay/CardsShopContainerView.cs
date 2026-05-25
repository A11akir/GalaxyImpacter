using System.Collections.Generic;
using Feature.ShopGamePlay.Script.ShopWindow;
using UnityEngine;

namespace Feature.ShopGamePlay
{
    public class CardsShopContainerView : MonoBehaviour
    {
        [SerializeField] private List<CardBuyShopView> _buyViews;

        public List<CardBuyShopView> GetBuyViews() => _buyViews;

        public void SetCards(List<CardStatsData> cards)
        {
            for (int i = 0; i < _buyViews.Count; i++)
            {
                if (i < cards.Count)
                {
                    _buyViews[i].SetView(cards[i]);
                    _buyViews[i].gameObject.SetActive(true);
                }
                else
                {
                    _buyViews[i].gameObject.SetActive(false);
                }
            }
        }
    }
}