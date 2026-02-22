using UnityEngine;

namespace Feature.Battlefield.Script.View
{
    public class TipPlaceBattlefieldViewSystem : MonoBehaviour
    {
        [SerializeField] private Transform[] _tipPositions;
        [SerializeField] private float _highlightAlpha = 0.5f;

        private int _indexNearCard;
        
        public int GetCardIndex() => _indexNearCard;
        
        private CanvasGroup[] _canvasGroups;

        private void Awake()
        {
            _canvasGroups = new CanvasGroup[_tipPositions.Length];
            
            for (int i = 0; i < _tipPositions.Length; i++)
            {
                _canvasGroups[i] = _tipPositions[i].GetComponent<CanvasGroup>();
                _canvasGroups[i].alpha = 0f;
            }
            
            gameObject.SetActive(false);
        }

        public void ActiveNearTip(Transform cardTransform)
        {
            gameObject.SetActive(true);
            
            float closestDistance = float.MaxValue;
            int closestIndex = -1;
            
            for (int i = 0; i < _tipPositions.Length; i++)
            {
                float distance = Vector3.Distance(cardTransform.position, _tipPositions[i].position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }
            
            for (int i = 0; i < _tipPositions.Length; i++)
            {
                _indexNearCard = closestIndex;
                _canvasGroups[i].alpha = (i == _indexNearCard) ? _highlightAlpha : 0f;
            }
        }

        public void Inactive()
        {
            for (int i = 0; i < _tipPositions.Length; i++)
            {
                _canvasGroups[i].alpha = 0f;
            }
            gameObject.SetActive(false);
        }
    }
}