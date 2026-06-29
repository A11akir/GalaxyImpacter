using UnityEngine;

namespace Feature.Hero.Script
{
    public class HeroPowerGameplayView : HeroPowerPreview
    {
        [SerializeField] private GameObject _heroPowerUsedThisTurnWindow;
        [SerializeField] private GameObject _canAvailableCastHeroPower;

        public void SetCanCastView(bool canCast) =>
            _canAvailableCastHeroPower.SetActive(canCast);

        public void SetUsedThisTurnView(bool usedThisTurn)
        {
            _heroPowerUsedThisTurnWindow.SetActive(usedThisTurn);
            _costFrame.SetActive(!usedThisTurn);
        }
        
        public override void SetPassiveView()
        {
            _costFrame.SetActive(false);
            _heroPowerUsedThisTurnWindow.SetActive(false);
            _canAvailableCastHeroPower.SetActive(false);
        }
    }
}