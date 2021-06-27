public static class Utility
{
    public static int IncrementInt(int value, int maxExclusiveValue)
    {
        if (value < 0)
            return maxExclusiveValue - 1;
        if (value >= maxExclusiveValue)
            return 0;

        return value;
    }

    public static int CalculateDamage(int offensiveValue, int defensiveValue)
    {
        return offensiveValue > defensiveValue ? offensiveValue - defensiveValue : 1;
    }
}
