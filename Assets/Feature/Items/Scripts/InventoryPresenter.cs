using Feature.GameSessionData;
using R3;

namespace Feature.Items.Scripts
{
    public class InventoryPresenter : System.IDisposable
    {
        private readonly InventoryView _inventoryView;
        private readonly GameSessionModel _gameSessionModel;
        private readonly CompositeDisposable _disposables = new();

        public InventoryPresenter(InventoryView inventoryView, GameSessionModel gameSessionModel)
        {
            _inventoryView = inventoryView;
            _gameSessionModel = gameSessionModel;
        }

        public void Init() => SubscribeToInventoryChanges();

        public void SubscribeToInventoryChanges()
        {
            _gameSessionModel.PlayerHero.Inventory.Items
                .Subscribe(items => _inventoryView.SetViews(items))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}