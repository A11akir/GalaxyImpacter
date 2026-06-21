namespace Feature.CardEffect.Script
{
    public enum TargetSelectionType
    {
        Target,        // context.Target — обычная явная цель (выбранная игроком при касте)
        Self,          // context.Caster
        All,           // вообще все живые сущности
        PlayerHero,
        EnemyHero,
        EnemyMinion,   // существа противника (без героя), единственное число — но список из них
        PlayerMinion,  // свои существа (без героя)
        Allies,        // свой герой + свои существа
        Enemies        // вражеский герой + его существа
    }
}