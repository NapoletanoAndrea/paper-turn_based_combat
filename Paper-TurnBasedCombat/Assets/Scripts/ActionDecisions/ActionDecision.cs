using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType { Self, Enemy }

public enum Stat { MaxHp, Hp, Atk, Def, Magic, MaxMana, Mana, Speed }

public abstract class ActionDecision : ScriptableObject
{
    public abstract float GetScore(StatsHandler attacker, StatsHandler target, Action action);
}
