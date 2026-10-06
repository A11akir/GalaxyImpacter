using System;
using R3;
using Feature.GameSessionData;
using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    [Serializable]
    public class FireDamageBonusValueSource : IDynamicCostValueSource
    {
        public IDisposable Subscribe(CardAndHealthEntityOwnerData owner, Action<int> onValueChanged)
        {
            var composite = new CompositeDisposable();
            var inner = new SerialDisposable();
            composite.Add(inner);

            owner.PassiveEffects.ActivePassives
                .Subscribe(_ =>
                {
                    var bonus = owner.PassiveEffects.Find<FireDamageBonus>();

                    if (bonus != null)
                        inner.Disposable = bonus.Value.Subscribe(onValueChanged);
                    else
                    {
                        inner.Disposable = null;
                        onValueChanged(0);
                    }
                })
                .AddTo(composite);

            return composite;
        }
    }
}