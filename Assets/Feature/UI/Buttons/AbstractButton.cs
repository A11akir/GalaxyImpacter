using System.Collections.Generic;
using UnityEngine;

namespace Feature.UI.Buttons
{
    public abstract class AbstractButton : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _showList;
        [SerializeField] private List<GameObject> _hideList;

        public void Execute()
        {
            Show();
            Hide();
            OnExecute();
        }

        protected abstract void OnExecute();

        private void Show()
        {
            foreach (var s in _showList)
                s.SetActive(true);
        }

        private void Hide()
        {
            foreach (var h in _hideList)
                h.SetActive(false);
        }
    }
}