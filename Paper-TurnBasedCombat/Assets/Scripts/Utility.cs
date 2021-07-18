using System.Collections.Generic;
using UnityEngine;

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
        return offensiveValue > defensiveValue ? offensiveValue - defensiveValue : 0;
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

    public static ref int GetStat(StatsHandler handler, Stat stat)
    {
        switch (stat)
        {
            case Stat.MaxHp: return ref handler.currentMaxHp;
            case Stat.Hp: return ref handler.currentHp;
            case Stat.Atk: return ref handler.currentAtk;
            case Stat.Def: return ref handler.currentDef;
            case Stat.MaxMana: return ref handler.currentMaxMana;
            case Stat.Magic: return ref handler.currentMagic;
            case Stat.Mana: return ref handler.currentMana;
            case Stat.Speed: return ref handler.currentSpeed;
        }

        return ref handler.currentMaxHp;
    }
    
    public static float ToScale(float x0, float x2, float y0, float y2, float x1)
    {
        float n = Mathf.Abs(x0) < Mathf.Abs(x2) ? x0 : x2;
        n = x1 - n;
        float r = n != 0 ? (Mathf.Abs(x0) + x2)/n : 0;
        float R = r != 0 ? 1 / r : 0;
        float t = (Mathf.Abs(y0) + y2) * R;
        return t + y0;
    }
}
