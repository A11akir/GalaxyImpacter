namespace Feature.GoogleSheets
{
    public interface IHeroStatsData
    {
        int Rarity { get; set; }
        int Cost { get; set; }
        string Name { get; set; }
    }
}