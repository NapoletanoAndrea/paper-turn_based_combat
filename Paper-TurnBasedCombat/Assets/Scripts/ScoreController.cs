using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] ActionContainer[] actionContainers;

    public void DoAction(StatsHandler attacker)
    {
        ActionContainer chosenAction = null;
        float highScore = -Mathf.Infinity;

        foreach(var aC in actionContainers)
        {
            var temp = aC.GetScore(attacker, aC.GetTarget(attacker));
            if (temp > highScore)
            {
                highScore = temp;
                chosenAction = aC;
            }
        }
        
        chosenAction?.DoAction(attacker, chosenAction?.GetTarget(attacker));
    }
}

[System.Serializable]
public class ActionContainer
{
    public Action action;
    public TargetScore[] targetScores;
    public DecisionScore[] decisions;

    public StatsHandler GetTarget(StatsHandler attacker)
    {
        StatsHandler target = null;
        float highScore = 0;

        foreach(var p in BattleSystem.instance.players)
        {
            float score = 0;

            foreach (var t in targetScores)
            {
                score += Utility.ToScale(0, 1, t.minScore, t.maxScore, t.condition.GetTargetScore(attacker, p, action));
            }
            
            if(score > highScore)
            {
                highScore = score;
                target = p;
            } 
        }

        return target != null ? target : attacker;
    }

    public float GetScore(StatsHandler attacker, StatsHandler target)
    {
        float score = 0;
        foreach(var d in decisions)
        {
            score += Utility.ToScale(0, 1, d.minScore, d.maxScore, d.decision.GetScore(attacker, target, action));
        }
        return score;
    }
    
    public void DoAction(StatsHandler attacker, StatsHandler target)
    {
        action.Act(new TargetChoiceParameter(attacker, target));
    }
}

[System.Serializable]
public class DecisionScore
{
    public ActionDecision decision;
    public float minScore;
    public float maxScore;
}

[System.Serializable]
public class TargetScore
{
    public TargetCondition condition;
    public float minScore;
    public float maxScore;
}