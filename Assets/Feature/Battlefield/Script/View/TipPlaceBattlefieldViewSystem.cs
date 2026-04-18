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
            
            _occupiedSlots = new bool[_tipPositions.Length];
            gameObject.SetActive(false);
        }

        private bool[] _occupiedSlots;

        public void OccupySlot(int index) => _occupiedSlots[index] = true;
        public void FreeSlot(int index) => _occupiedSlots[index] = false;

        public void ActiveNearTip(Transform cardTransform)
        {
            gameObject.SetActive(true);
    
            float closestDistance = float.MaxValue;
            int closestIndex = -1;
    
            for (int i = 0; i < _tipPositions.Length; i++)
            {
                if (_occupiedSlots[i]) continue;
        
                float distance = Vector3.Distance(cardTransform.position, _tipPositions[i].position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            if (closestIndex == -1)
            {
                Inactive();
                return;
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