using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/Taunt")]
public class Taunt : Skill
{
    [SerializeField] int duration;

    public override void Act(ActionParameter actionParameter)
    {
        bool success = new HasTaunted(duration).InflictStatus(actionParameter.attacker);
        string text = string.Empty;

        if (success)
        {
            text = actionParameter.attacker.name + " has taunted the enemies";
            BattleSystem.instance.EnterDialogueState(text, actionParameter.attacker.name + " is now the target of enemy attacks");
        }
        else
        {
            text = "Enemies are already taunted";
            BattleSystem.instance.EnterDialogueState(text);
        }

        base.Act(actionParameter);
    }
}
