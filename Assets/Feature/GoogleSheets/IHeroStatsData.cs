namespace Feature.GoogleSheets
{
    public interface IHeroStatsData
    {
        int Health { get; set; }
        int Cost { get; set; }
        string Name { get; set; }
    }
}