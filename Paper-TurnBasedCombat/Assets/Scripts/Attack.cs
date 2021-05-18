using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Basic Actions/Attack")]
public class Attack : Action
{
    public override void Act(StatsHandler attacker, StatsHandler target)
    {
        target.TakeDamage(attacker.currentAtk);
    }
}
