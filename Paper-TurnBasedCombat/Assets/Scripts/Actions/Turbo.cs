using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Basic Actions/Turbo")]

public class Turbo : Action
{
    public override void Act(ActionParameter actionParameter)
    {
        actionParameter.attacker.turboCounter++;
        BattleSystem.instance.EnterDialogueState(actionParameter.attacker.name + " now holds " + actionParameter.attacker.turboCounter + " turbo counter");
    }
}
