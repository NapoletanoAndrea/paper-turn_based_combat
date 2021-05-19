using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : Action
{
    public int manaConsumed;
    public string skillDescription;

    public override void Act(StatsHandler attacker, StatsHandler target)
    {
        attacker.currentMana = Mathf.Clamp(attacker.currentMana - manaConsumed, 0, attacker.currentMaxMana);
    }
}
