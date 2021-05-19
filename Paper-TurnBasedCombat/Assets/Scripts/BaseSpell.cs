using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Spell", menuName = "ScriptableObjects/Basic Actions/Base Spell")]

public class BaseSpell : Action
{
    public override void Act(StatsHandler attacker, StatsHandler target)
    {
        target.TakeDamage(attacker.currentMagic);
    }
}
