using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string actionName;
    public abstract void Act(ActionParameter actionParameter);

    public virtual int GetDamage(StatsHandler attacker, StatsHandler target)
    {
        return 0;
    }

    public virtual int GetAttackStat(StatsHandler attacker)
    {
        return 0;
    }
}
