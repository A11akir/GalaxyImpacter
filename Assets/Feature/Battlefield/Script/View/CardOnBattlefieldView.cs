using System;
using Feature.GoogleSheets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardOnBattlefieldView : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private TextMeshProUGUI _healthBoard;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Image _iconMinionHand;
    [SerializeField] private Image _iconMinionBoard;
    [SerializeField] private GameObject _borderHasAction;
    [SerializeField] private GameObject _heroDescriptionWindow;
    [SerializeField] private GameObject _selectEntityView; // ← новый

    public event Action OnClicked; // ← новый

    public void SetDataView(MinionCardData cardStatsData)
    {
        gameObject.SetActive(true);
        _heroDescriptionWindow.SetActive(false);
        _health.text = cardStatsData.Health.ToString();
        _healthBoard.text = cardStatsData.Health.ToString();
        _iconMinionHand.sprite = cardStatsData.IconImage;
        _iconMinionBoard.sprite = cardStatsData.IconImage;
        _name.text = cardStatsData.Name;
        _cost.text = cardStatsData.Cost.ToString();
    }

    public void SetCanHasAction(bool canCast) => _borderHasAction.SetActive(canCast);
    public void SetSelected(bool selected) => _selectEntityView.SetActive(selected); // ← новый

    public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(); // ← новый

    public void OnPointerEnter(PointerEventData eventData) => _heroDescriptionWindow.SetActive(true);
    public void OnPointerExit(PointerEventData eventData) => _heroDescriptionWindow.SetActive(false);
}