using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TargetConditions/IsKillable")]
public class IsKillable : TargetCondition
{
    public override float GetTargetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        if (target.currentHp <= action.GetDamage(attacker, target) && !target.IsDead())
            return 1;

        return 0;
    }
}