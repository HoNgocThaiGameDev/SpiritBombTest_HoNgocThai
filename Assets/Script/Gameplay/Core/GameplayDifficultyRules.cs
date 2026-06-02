public static class GameplayDifficultyRules
{
    public static float GetEnemyHealthScale(int level)
    {
        if (level == 1)
            return 1.35f;
        if (level == 3)
            return 1.1f;
        return 1f;
    }

    public static float GetEnemyAttackScale(int level)
    {
        if (level == 1)
            return 0.35f;
        if (level == 2)
            return 1.05f;
        if (level == 3)
            return 1.12f;
        return 1f;
    }

    public static float GetEnemyFireRateMultiplier(int level)
    {
        if (level == 1)
            return 1.8f;
        if (level == 2)
            return 0.92f;
        if (level == 3)
            return 0.85f;
        return 1f;
    }

    public static bool CanEnemyShoot(int level, EnemyMovementMode movementMode, int rowPosition)
    {
        if (level == 1
            && (movementMode == EnemyMovementMode.FormationHold || movementMode == EnemyMovementMode.FormationSine))
        {
            return rowPosition == 0;
        }

        return true;
    }
}
