using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType { Self, Enemy }

public enum Stat { MaxHp, Hp, Atk, Def, Magic, MaxMana, Mana, Speed }

public abstract class ActionDecision : ScriptableObject
{
    public abstract float GetScore(StatsHandler attacker, StatsHandler target);

    public float GetStat(StatsHandler handler, Stat stat)
    {
        switch (stat)
        {
            case Stat.MaxHp: return handler.currentMaxHp;
            case Stat.Hp: return handler.currentHp;
            case Stat.Atk: return handler.currentAtk;
            case Stat.Def: return handler.currentDef;
            case Stat.MaxMana: return handler.currentMaxMana;
            case Stat.Magic: return handler.currentMagic;
            case Stat.Mana: return handler.currentMana;
            case Stat.Speed: return handler.currentSpeed;
        }

        return 0f;
    }
}
