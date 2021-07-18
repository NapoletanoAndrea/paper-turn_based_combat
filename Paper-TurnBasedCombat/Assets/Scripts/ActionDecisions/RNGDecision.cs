using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionDecisions/RNGDecision")]

public class RNGDecision : ActionDecision
{
    public override float GetScore(StatsHandler attacker, StatsHandler target, Action action)
    {
        return Random.Range(0f, 1f);
    }
}
