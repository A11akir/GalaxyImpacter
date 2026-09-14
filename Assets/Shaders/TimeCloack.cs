using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TimeClock : MonoBehaviour
{
    [SerializeField] private Image _runeFill;
    [SerializeField] private Image _runeGlow;
    [SerializeField] private float _glowOffset = 0.15f;

    [Range(0f, 1f)]
    [SerializeField] private float _fillAmount = 0.5f;

    private Material _matFill;
    private Material _matGlow;
    private bool _initialized;

    void Awake()
    {
        _matFill = Instantiate(_runeFill.material);
        _matGlow = Instantiate(_runeGlow.material);
        _runeFill.material = _matFill;
        _runeGlow.material = _matGlow;
        _initialized = true;
        SetFillAmount(_fillAmount);
    }

    private void OnValidate()
    {
        // в редакторе меняем оригинальный материал напрямую
        if (!Application.isPlaying)
        {
            if (_runeFill != null && _runeFill.material != null)
                _runeFill.material.SetFloat("_FillAmount", _fillAmount);
            if (_runeGlow != null && _runeGlow.material != null)
                _runeGlow.material.SetFloat("_FillAmount", Mathf.Clamp01(_fillAmount + _glowOffset));
            return;
        }

        // в рантайме меняем клоны
        if (_initialized)
            SetFillAmount(_fillAmount);
    }

    public void SetFillAmount(float amount)
    {
        _fillAmount = amount;
        _matFill.SetFloat("_FillAmount", amount);
        _matGlow.SetFloat("_FillAmount", Mathf.Clamp01(amount + _glowOffset));
    }
}