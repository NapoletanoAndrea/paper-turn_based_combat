using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompareNum { LessThan, MoreThan }

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/HpPercentage")]

public class HpPercentage : ActionDecision
{
    [SerializeField] TargetType targetType;
    [SerializeField] CompareNum compare;
    [SerializeField] float percentage;

    public override float GetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        int maxHp;
        int currentHp;

        if(targetType == TargetType.Self)
        {
            maxHp = attacker.currentMaxHp;
            currentHp = attacker.currentHp;
        }
        else
        {
            maxHp = target.currentMaxHp;
            currentHp = target.currentHp;
        }

        float percentageHp = Utility.ToScale(0, maxHp, 0, 100, currentHp);
        float score;

        if (compare == CompareNum.LessThan)
            score = percentageHp <= percentage ? 1 : 0;
        else
            score = percentageHp >= percentage ? 1 : 0;

        return score;
    }
}
