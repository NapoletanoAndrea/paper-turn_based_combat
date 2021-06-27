using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Basic Actions/Attack")]
public class Attack : Action
{
    public override void Act(ActionParameter actionParameter)
    {
        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterEnemyTargetState();
                break;
            case TargetChoiceParameter p:
                int damage = Utility.CalculateDamage(p.attacker.currentAtk, p.target.currentDef);
                p.target.TakeDamage(damage);

                string dmgText = p.attacker.name + " deals " + damage + " damage to " + p.target.name + ".";
                BattleSystem.instance.EnterDialogueState(dmgText);
                break;
        }
    }
}
