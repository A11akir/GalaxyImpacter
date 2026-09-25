using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Card.Script
{
    public abstract class EffectIconViewBase : MonoBehaviour
    {
        [SerializeField] protected Image _icon;
        [SerializeField] protected TextMeshProUGUI _valueText;

        protected string _description;
        protected string _nameEffect;

        public bool IsInUse { get; protected set; }

        public virtual void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
            IsInUse = true;
        }

        public virtual void SetValue(int? value) =>
            _valueText.text = value.HasValue ? value.Value.ToString() : "";

        public void HideValue() => _valueText.text = "";

        public virtual void SetDescription(string text) => _description = text;
        public void SetName(string name) => _nameEffect = name;

        public abstract void ForceHide();
    }
}