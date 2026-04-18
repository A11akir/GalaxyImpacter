using Feature.GameSessionData;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        public void TakeDamage(CardAndHealthEntityOwnerData target, int damage)
        {
            target.HealthValue -= damage;
        }
    }
}