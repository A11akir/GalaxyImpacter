namespace Feature.GoogleSheets
{
    public interface IHeroStatsData
    {
        int Health { get; set; }
        int HeroPowerCost { get; set; }
        string Name { get; set; }
    }
}