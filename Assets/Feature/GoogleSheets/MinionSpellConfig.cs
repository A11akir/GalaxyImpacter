using System.Collections.Generic;

namespace Feature.GoogleSheets
{
    public class MinionSpellConfig
    {
        public string Name;
        public int Cost;
        public List<int> Values = new();
        public string Description;
        public string MinionNameOwner;
        public string Type;
    }
}