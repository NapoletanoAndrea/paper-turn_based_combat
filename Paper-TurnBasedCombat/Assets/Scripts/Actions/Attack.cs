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
                int damage = GetDamage(p.attacker, p.target);
                p.target.TakeDamage(damage);

                string dmgText = p.attacker.name + " deals " + damage + " damage to " + p.target.name + ".";
                BattleSystem.instance.EnterDialogueState(dmgText);
                break;
        }
    }

    public override int GetDamage(StatsHandler attacker, StatsHandler target)
    {
        return Utility.CalculateDamage(attacker.currentAtk, target.currentDef);
    }

    public override int GetAttackStat(StatsHandler attacker)
    {
        return attacker.currentAtk;
    }
}
