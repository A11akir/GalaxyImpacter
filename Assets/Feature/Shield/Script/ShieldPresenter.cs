using Feature.GameSessionData;
using Feature.Shield.Script;
using R3;

namespace Feature.Shield
{
    public class ShieldPresenter
    {
        private readonly CompositeDisposable _disposables = new();

        public ShieldPresenter(CardAndHealthEntityOwnerData owner, ShieldView view)
        {
            owner.Shield
                .Subscribe(value => view.SetShield(value))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}
