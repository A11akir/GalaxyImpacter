using System.Collections.Generic;
using System.Linq;
using Feature.Battlefield.Script;
using Feature.Battlefield.Script.View;
using Feature.Card.Script;
using Feature.GameSessionData;
using Feature.Hero;
using R3;
using UnityEngine;
using Zenject;

public class BattlefieldSystem : MonoBehaviour
{
    private TipPlaceBattlefieldViewSystem _tipPlaceBattlefieldViewSystem;
    private CardOnBattlefieldPresenter _cardOnBattlefieldPresenter;
    private GameSessionModel _gameSessionModel;
    private CreateOwnerCardAndHealthEntitySystem _createOwnerCardAndHealthEntitySystem;

    [SerializeField] private GameObject enemyBattlefield;
    [SerializeField] private GameObject playerBattlefield;

    private readonly Dictionary<GameSessionPlayerData, List<CardOnBattlefieldView>> _battlefieldViews = new();
    private readonly Dictionary<GameSessionPlayerData, List<MinionCardData>> _previousCards = new();

    [Inject]
    public void Construct(GameSessionModel gameSessionModel,
        CardOnBattlefieldPresenter cardOnBattlefieldPresenter,
        TipPlaceBattlefieldViewSystem tipPlaceBattlefieldViewSystem,
        CreateOwnerCardAndHealthEntitySystem createOwnerCardAndHealthEntitySystem)
    {
        _gameSessionModel = gameSessionModel;
        _cardOnBattlefieldPresenter = cardOnBattlefieldPresenter;
        _tipPlaceBattlefieldViewSystem = tipPlaceBattlefieldViewSystem;
        _createOwnerCardAndHealthEntitySystem = createOwnerCardAndHealthEntitySystem;
    }

    public void Init()
    {
        _battlefieldViews[_gameSessionModel.PlayerHero] = GetCardViewsFromBattlefield(playerBattlefield);
        _battlefieldViews[_gameSessionModel.EnemyHero] = GetCardViewsFromBattlefield(enemyBattlefield);

        _previousCards[_gameSessionModel.PlayerHero] = new();
        _previousCards[_gameSessionModel.EnemyHero] = new();

        SubscribeReactiveBoardList(_gameSessionModel.PlayerHero);
        SubscribeReactiveBoardList(_gameSessionModel.EnemyHero);
    }

    private List<CardOnBattlefieldView> GetCardViewsFromBattlefield(GameObject battlefield)
    {
        var cardViews = new List<CardOnBattlefieldView>();
        foreach (Transform child in battlefield.transform)
        {
            var cardView = child.GetComponent<CardOnBattlefieldView>();
            cardViews.Add(cardView);
        }
        return cardViews;
    }

    private void SubscribeReactiveBoardList(GameSessionPlayerData playerData)
    {
        playerData.CardsInBoard
            .Subscribe(currentCards =>
            {
                var nonNullCards = currentCards.Where(c => c != null).ToList();
                var previousNonNullCards = _previousCards[playerData].Where(c => c != null).ToList();

                var addedCards = nonNullCards
                    .Where(c => previousNonNullCards.All(p => p.id != c.id))
                    .ToList();

                var removedCards = previousNonNullCards
                    .Where(p => nonNullCards.All(c => c.id != p.id))
                    .ToList();

                if (removedCards.Count > 0)
                    OnCardRemovedFromBoard(removedCards[0], playerData);

                if (addedCards.Count > 0)
                {
                    int addedIndex = currentCards.FindIndex(c => c != null && c.id == addedCards[0].id);
                    OnCardAddedBoard(addedCards[0], addedIndex, playerData);
                }

                _previousCards[playerData] = currentCards.ToList();
            });
    }

    private void OnCardAddedBoard(MinionCardData addedCard, int addedIndex, GameSessionPlayerData playerData)
    {
        var views = _battlefieldViews[playerData];
        _cardOnBattlefieldPresenter.SetCardInPlayerHand(views[addedIndex], addedCard);
        playerData.MainHeroEntity().RemoveCardFromHand(addedCard);
        
        var newOwner = new CardAndHealthEntityOwnerData();
        playerData.CardAndHealthEntityOwners.Add(newOwner);
        _createOwnerCardAndHealthEntitySystem.CreateEntity(newOwner);
    }

    private void OnCardRemovedFromBoard(MinionCardData removedCard, GameSessionPlayerData playerData)
    {
    }

    public void AddCardInBattlefield(GameSessionPlayerData playerData, CardStatsData cardData)
    {
        var data = new MinionCardData();
        data = (MinionCardData)cardData;
        playerData.AddCardToBoard(data, _tipPlaceBattlefieldViewSystem.GetCardIndex());
    }
}