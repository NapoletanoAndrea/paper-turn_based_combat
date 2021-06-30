using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] ActionContainer[] actionContainers;

    public void DoAction(StatsHandler attacker, StatsHandler target)
    {
        Action chosenAction = null;
        float temp = 0;
        float highScore = 0;

        foreach(var aC in actionContainers)
        {
            foreach(var decision in aC.decisions)
                temp += decision.GetScore(attacker, target);

            if (temp > highScore)
            {
                highScore = temp;
                chosenAction = aC.action;
            }

            temp = 0;
        }

        chosenAction?.Act(new TargetChoiceParameter(attacker, target));
    }
}

[System.Serializable]
public class ActionContainer
{
    public Action action;
    public ActionDecision[] decisions;
}
