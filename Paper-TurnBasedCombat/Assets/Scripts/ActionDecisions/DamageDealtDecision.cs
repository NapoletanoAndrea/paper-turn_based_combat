using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/DamageDealt")]

public class DamageDealtDecision : ActionDecision
{
    [SerializeField] Stat attackStat;
    [SerializeField] AnimationCurve curve;

    public override float GetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        int damage = action.GetDamage(attacker, target); 
        float score = Utility.ToScale(0, action.GetAttackStat(attacker), 0, 1, damage);
        score = curve.Evaluate(score);
        return score;
    }
}
