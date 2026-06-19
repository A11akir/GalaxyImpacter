using R3;

namespace Feature.CardEffect.Script
{
    public interface IValueProvider
    {
        ReadOnlyReactiveProperty<int> Value { get; }
    }
}