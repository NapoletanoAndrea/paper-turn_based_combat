using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/ManaDecision")]

public class ManaDecision : ActionDecision
{
    public override float GetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        Skill s = action as Skill;
        if (s == null) return 1;

        if (attacker.currentMana >= s.manaConsumed)
            return 1;

        return 0;
    }
}
