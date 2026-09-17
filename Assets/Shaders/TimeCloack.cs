using UnityEngine;
using UnityEngine.UI;

namespace Shaders
{
    public class TimeClockView : MonoBehaviour
    {
        [SerializeField] private Image _runeFill;
        [SerializeField] private Image _runeGlow;
        [SerializeField] private float _glowOffset = 0.10f;

        [SerializeField] private Material _originalFillMaterial;
        [SerializeField] private Material _originalGlowMaterial;

        [Range(0f, 1f)] [SerializeField] private float _fillAmount = 0.5f;
        [Range(0f, 1f)] [SerializeField] private float _glowAmount = 0.5f;
        [Range(0f, 1f)] [SerializeField] private float _bothAmount = 0.5f;

        private Material _matFill;
        private Material _matGlow;
        private bool _initialized;

        void Awake()
        {
            _matFill = Instantiate(_originalFillMaterial);
            _matGlow = Instantiate(_originalGlowMaterial);
            _runeFill.material = _matFill;
            _runeGlow.material = _matGlow;
            _initialized = true;
            SetFillAmount(_bothAmount);
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                if (_originalFillMaterial != null)
                    _originalFillMaterial.SetFloat("_FillAmount", _fillAmount);
                if (_originalGlowMaterial != null)
                    _originalGlowMaterial.SetFloat("_FillAmount", _glowAmount);
                return;
            }

            if (!_initialized) return;

            _matFill.SetFloat("_FillAmount", _fillAmount);
            _matGlow.SetFloat("_FillAmount", _glowAmount);
        }

        public void SetFillAmount(float amount)
        {
            _bothAmount = amount;
            _fillAmount = amount;
            _glowAmount = Mathf.Clamp01(amount + _glowOffset);

            if (!_initialized) return;

            _matFill.SetFloat("_FillAmount", _fillAmount);
            _matGlow.SetFloat("_FillAmount", _glowAmount);
        }

        public void SetAlphaFadeLength(float length)
        {
            if (!_initialized) return;
            _matFill.SetFloat("_AlphaFadeLength", length);
            _matGlow.SetFloat("_AlphaFadeLength", length);
        }
    }
}