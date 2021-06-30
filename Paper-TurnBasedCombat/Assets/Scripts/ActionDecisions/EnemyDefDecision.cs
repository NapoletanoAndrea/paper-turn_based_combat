using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/EnemyDefence")]

public class EnemyDefDecision : ActionDecision
{
    [SerializeField] float minScore;
    [SerializeField] float maxScore;

    [SerializeField] AnimationCurve curve;

    public override float GetScore(StatsHandler attacker, StatsHandler target)
    { 
        float score = Utility.Proportion(target.currentDef, attacker.currentAtk, 1);
        score = curve.Evaluate(score);
        score = Utility.Proportion(score, 1, maxScore);
        return score >= minScore ? score : minScore;
    }
}
