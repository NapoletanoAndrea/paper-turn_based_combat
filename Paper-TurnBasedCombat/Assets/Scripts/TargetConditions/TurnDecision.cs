using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TargetConditions/TurnDecision")]

public class TurnDecision : TargetCondition
{
    public override float GetTargetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        float score = 0;

        int attackerIndex = 0;
        int targetIndex = 0;

        for(int i = 0; i < BattleSystem.instance.actors.Count; i++)
        {
            if (BattleSystem.instance.actors[i] == attacker)
                attackerIndex = i;
            if (BattleSystem.instance.actors[i] == target)
                targetIndex = i;
        }

        for (int i = attackerIndex; i < BattleSystem.instance.actors.Count + attackerIndex; i++)
        {
            int index = i;
            if (i >= BattleSystem.instance.actors.Count)
                index = i - BattleSystem.instance.actors.Count;

            if(index == targetIndex)
            {
                score = 1 / (BattleSystem.instance.actors.Count - 1) * (i - attackerIndex - 1);
                break;
            }
        }

        return score;
    }
}
