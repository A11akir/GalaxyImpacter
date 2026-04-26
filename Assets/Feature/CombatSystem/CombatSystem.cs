using Feature.GameSessionData;

namespace Feature.CombatSystem
{
    public class CombatSystem
    {
        public void TakeDamage(CardAndHealthEntityOwnerData target, int damage, CardAndHealthEntityOwnerData source)
        {
            if (target == null) return;
        
            target.LastDamageSource = source;
            target.HealthValue -= damage;
        }
    }
}