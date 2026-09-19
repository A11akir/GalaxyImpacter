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
            
            foreach (var go in _costFrame)
                go.SetActive(!usedThisTurn);
        }
        
        public override void SetPassiveView()
        {
            foreach (var go in _costFrame)
                go.SetActive(false);
            _heroPowerUsedThisTurnWindow.SetActive(false);
            _canAvailableCastHeroPower.SetActive(false);
        }
    }
}               