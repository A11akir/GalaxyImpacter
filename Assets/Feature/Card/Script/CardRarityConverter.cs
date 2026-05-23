
namespace Feature.Card.Script
{
    public static class CardRarityConverter
    {
        public static CardRarity FromString(string rarity)
        {
            return rarity?.Trim().ToLower() switch
            {
                "common"    => CardRarity.Common,
                "hidden"    => CardRarity.Hidden,
                "anomalous" => CardRarity.Anomalous,
                "primordial"=> CardRarity.Primordial,
                _           => CardRarity.None
            };
        }
        
        public static string ToString(CardRarity rarity)
        {
            return rarity switch
            {
                CardRarity.Common    => "Common",
                CardRarity.Hidden    => "Hidden",
                CardRarity.Anomalous => "Anomalous",
                CardRarity.Primordial=> "Primordial",
                _                   => "None"
            };
        }
    }
}