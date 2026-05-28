using System.Collections.Generic;

namespace Feature.GameSessionData
{
    public class HeroPowerUsageTracker
    {
        private readonly List<bool> _usedThisTurn = new();

        public void Init(int count)
        {
            _usedThisTurn.Clear();
            for (int i = 0; i < count; i++)
                _usedThisTurn.Add(false);
        }

        public void Reset()
        {
            for (int i = 0; i < _usedThisTurn.Count; i++)
                _usedThisTurn[i] = false;
        }

        public bool IsUsed(int index) =>
            index < _usedThisTurn.Count && _usedThisTurn[index];

        public void SetUsed(int index)
        {
            if (index < _usedThisTurn.Count)
                _usedThisTurn[index] = true;
        }
    }
}