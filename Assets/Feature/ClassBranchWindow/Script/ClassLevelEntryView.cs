using Feature.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassLevelEntryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _orb;

    private AllHeroClass _heroClass; // ← хранится динамически

    public AllHeroClass HeroClass => _heroClass;

    public void SetView(AllHeroClass heroClass, int level, Color orbColor)
    {
        _heroClass = heroClass;
        _levelText.text = level.ToString();
        _orb.color = orbColor;
    }
}