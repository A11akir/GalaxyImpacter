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
            IDisposable innerSubscription = null;

            owner.PassiveEffects.ActivePassives
                .Subscribe(_ =>
                {
                    innerSubscription?.Dispose();

                    var bonus = owner.PassiveEffects.Find<FireDamageBonus>();

                    if (bonus != null)
                        innerSubscription = bonus.Value.Subscribe(onValueChanged);
                    else
                        onValueChanged(0);
                })
                .AddTo(composite);

            return composite;
        }
    }
}