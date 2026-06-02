public static class EnemyDropRules
{
    public static int GetConfiguredItemIndex(int itemDropId)
    {
        return itemDropId >= 1 && itemDropId <= 8
            ? itemDropId - 1
            : -1;
    }

    public static int GetRandomItemIndex(int roll)
    {
        if (roll >= 0 && roll < 5)
            return 0;
        if (roll < 10)
            return 1;
        if (roll < 15)
            return 2;
        if (roll < 20)
            return -1;
        if (roll < 25)
            return 3;
        if (roll < 30)
            return 4;
        if (roll < 35)
            return 5;
        if (roll < 40)
            return 6;
        return -1;
    }
}
