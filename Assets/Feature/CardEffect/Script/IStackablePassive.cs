using Feature.PassiveEffect.Script;

namespace Feature.CardEffect.Script
{
    public interface IStackablePassive
    {
        void AddBonus(int amount);

        /// <summary>Можно ли влить приходящий эффект (context) в этот экземпляр.</summary>
        bool CanStackWith(EffectContext context);

        /// <summary>Длительность приходящего стака — части с разным сроком жизни хранятся отдельно.</summary>
        void PrepareStack(DurationType duration);
    }
}
