using System.Collections.Generic;
using Feature.Card.Script;
using Feature.GoogleSheets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Feature.Hero
{
    public class HeroPowerGameplayView : HeroPowerPreview
    {
        [SerializeField] private GameObject _chakraCostFrame;
        [SerializeField] private GameObject _heroPowerUsedThisTurnWindow;
        [SerializeField] private GameObject _canAvailableCastHeroPower;

        public void SetCanCastView(bool canCast) =>
            _canAvailableCastHeroPower.SetActive(canCast);

        public void SetUsedThisTurnView(bool usedThisTurn)
        {
            _heroPowerUsedThisTurnWindow.SetActive(usedThisTurn);
            _chakraCostFrame.SetActive(!usedThisTurn);
        }
    }
}