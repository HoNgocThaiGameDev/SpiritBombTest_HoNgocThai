using UnityEngine;

public static class PlayerDamageRules
{
    public static float GetEnemyProjectileDamage(int projectileDamage, int defense, float bonusDamage)
    {
        if (projectileDamage > defense)
        {
            float damage = projectileDamage - defense;
            return damage <= 10 ? 30 + bonusDamage : damage;
        }

        return projectileDamage < defense
            ? 20 + bonusDamage
            : 30 + bonusDamage;
    }

    public static float GetHeavyProjectileDamage(int defense, float totalEnergy, float bonusDamage)
    {
        return 110 > defense
            ? 110 - defense + bonusDamage
            : Mathf.Max(0f, totalEnergy / 10f);
    }

    public static float GetEnemyCollisionDamage(float bonusDamage)
    {
        return 20 + bonusDamage;
    }
}
