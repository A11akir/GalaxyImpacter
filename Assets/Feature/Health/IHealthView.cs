namespace Feature.Health
{
    public interface IHealthView
    {
        void SetHealth(int hp);
        void SetArmor(int armor);
    }
}