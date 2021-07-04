using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetCondition : ScriptableObject
{
    public abstract float GetTargetScore(StatsHandler attacker, StatsHandler target, Action action);
}
