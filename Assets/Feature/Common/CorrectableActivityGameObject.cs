using UnityEngine;

namespace Feature.Common
{
    public class CorrectableActivityGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] _shows;
        [SerializeField] private GameObject[] _hides;

        public void SetCorrectableActivityGameObject()
        {
            foreach (var go in _shows) go.SetActive(true);
            foreach (var go in _hides) go.SetActive(false);
        }
    }
}