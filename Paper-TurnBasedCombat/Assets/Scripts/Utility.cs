using System.Collections.Generic;

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

    public static float Average(List<float> values, int numOfValues)
    {
        float sum = 0;

        foreach (float value in values)
            sum += value;

        return sum / numOfValues;
    }

    public static float Proportion(float x1, float x2, float y)
    {
        return x1 * y / x2;
    }
}
