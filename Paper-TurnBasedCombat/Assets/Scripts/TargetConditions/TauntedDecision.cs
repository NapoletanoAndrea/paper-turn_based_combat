using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TargetConditions/TauntedDecision")]

public class TauntedDecision : TargetCondition
{
    public override float GetTargetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        foreach(var status in target.inflictedStatuses)
        {
            if (status is HasTaunted)
                return 1;
        }

        return 0;
    }
}
