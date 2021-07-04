using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/KillableDecision")]
public class KillableDecision : ActionDecision
{
    public override float GetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        int damage = action.GetDamage(attacker, target);
        if (target.currentHp <= damage && !target.IsDead())
            return 1;
        else
            return 0;
    }
}
