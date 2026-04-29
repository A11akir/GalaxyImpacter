using System.Collections.Generic;
using R3;

namespace Feature.Items.Scripts
{
    public class PlayerInventory
    {
        private readonly ReactiveProperty<List<ItemData>> _items = new(new List<ItemData>());
        public ReadOnlyReactiveProperty<List<ItemData>> Items => _items;

        public void AddItem(ItemData item)
        {
            var newList = new List<ItemData>(_items.Value);
            newList.Add(item);
            _items.Value = newList;
        }

        public void RemoveItem(ItemData item)
        {
            var newList = new List<ItemData>(_items.Value);
            newList.Remove(item);
            _items.Value = newList;
        }

        public bool HasItem(ItemData item)
        {
            return _items.Value.Contains(item);
        }

        public IEnumerable<ItemData> GetAllItems()
        {
            return _items.Value;
        }

        public void ClearInventory()
        {
            _items.Value = new List<ItemData>();
        }
    }
}