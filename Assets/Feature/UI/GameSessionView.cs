using Feature.Card.Script;
using UnityEngine;

namespace Feature.UI
{
    public class GameSessionView : MonoBehaviour
    {
        [SerializeField] public HandCardViews _enemyHandCardViews;
        [SerializeField] public HeroView _heroView;
        [SerializeField] public HeroView _enemyView;
    }
}