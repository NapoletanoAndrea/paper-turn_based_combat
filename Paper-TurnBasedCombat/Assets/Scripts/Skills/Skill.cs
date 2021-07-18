using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : Action
{
    public int manaConsumed;
    [TextArea] public string skillDescription;

    public override void Act(ActionParameter actionParameter)
    {
        if (actionParameter.attacker.currentMana < manaConsumed) return;
        actionParameter.attacker.currentMana = Mathf.Clamp(actionParameter.attacker.currentMana - manaConsumed, 0, actionParameter.attacker.currentMaxMana);
    }
}
