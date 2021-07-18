using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TargetConditions/StatDecision")]
public class StatDecision : TargetCondition
{
    [SerializeField] Stat stat;
    [SerializeField] int maxStatValue;
    [SerializeField] AnimationCurve curve;

    public override float GetTargetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        float score = Utility.ToScale(0, maxStatValue, 0, 1, Utility.GetStat(target, stat));
        score = curve.Evaluate(score);
        return score;
    }
}
