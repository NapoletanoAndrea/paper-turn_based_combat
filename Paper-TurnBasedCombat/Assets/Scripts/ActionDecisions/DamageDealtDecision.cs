using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/DamageDealt")]

public class DamageDealtDecision : ActionDecision
{
    [SerializeField] float minScore;
    [SerializeField] float maxScore;

    [SerializeField] Stat attackStat;

    [SerializeField] AnimationCurve curve;

    public override float GetScore(StatsHandler attacker, StatsHandler target)
    {
        int damage = Utility.CalculateDamage((int)GetStat(attacker, attackStat), target.currentDef); 
        float score = Utility.Proportion(damage, 30, 1);
        score = curve.Evaluate(score);
        score = Utility.Proportion(score, 1, maxScore);
        return score >= minScore ? score : minScore;
    }
}
