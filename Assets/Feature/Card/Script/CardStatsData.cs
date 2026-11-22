using System.Collections.Generic;
using Feature.Card.Script;
using Feature.CardEffect.Script;
using Feature.GoogleSheets;
using Feature.Hero;
using R3;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroStatsData", menuName = "Configs/Card/Card Stats Data", order = 1)]
public class CardStatsData : ScriptableObject, ICardStatsData
{
    [SerializeField] private string _name;
    [SerializeField] private CardRarity _rarity;
    [SerializeField] private Sprite _iconImage;
    [SerializeField] private List<AllHeroClass> _specialization;
    [SerializeField] private int _level;
    [SerializeField] private TargetType targetType;
    [SerializeField] private bool _inCollection;

    [SerializeField] private int _baseCost;
    [SerializeReference] private List<PassiveCardEffect> _passiveCardEffects = new();

    public int BaseCost { get => _baseCost; set => _baseCost = value; }

    public List<PassiveCardEffect> PassiveCardEffects => _passiveCardEffects;

    public string id = System.Guid.NewGuid().ToString();

    /// <summary>
    /// id карты в коллекции (BaseDeck). У копий после рефила сохраняется,
    /// чтобы карту можно было найти/сжечь в коллекции по id.
    /// </summary>
    [System.NonSerialized] public string SourceId;
    
    [HideInInspector] public CardOrigin Origin = CardOrigin.Permanent;

    public virtual bool IsHero => false;
    public string Name { get => _name; set => _name = value; }

    private ReactiveProperty<int> _costReactive;
    public ReadOnlyReactiveProperty<int> CostReactive => _costReactive ??= new ReactiveProperty<int>(_baseCost);

    public int Cost => _costReactive != null ? _costReactive.CurrentValue : _baseCost;
    
    public void SetCost(int value)
    {
        _costReactive ??= new ReactiveProperty<int>(_baseCost);
        _costReactive.Value = value;
    }

    public TargetType TargetType { get => targetType; set => targetType = value; }
    public CardRarity Rarity { get => _rarity; set => _rarity = value; }
    public List<AllHeroClass> Specialization { get => _specialization; set => _specialization = value; }
    public int Level { get => _level; set => _level = value; }
    public Sprite IconImage { get => _iconImage; set => _iconImage = value; }
    public bool InCollection { get => _inCollection; set => _inCollection = value; }
}
