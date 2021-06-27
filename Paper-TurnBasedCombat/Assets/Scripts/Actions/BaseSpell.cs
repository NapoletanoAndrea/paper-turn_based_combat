using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Spell", menuName = "ScriptableObjects/Basic Actions/Base Spell")]

public class BaseSpell : Action
{
    public override void Act(ActionParameter actionParameter)
    {
        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterEnemyTargetState();
                break;
            case TargetChoiceParameter p:
                p.target.TakeDamage(p.attacker.currentMagic);
                break;
        }
    }
}
