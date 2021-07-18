using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/Magic Sword")]
public class MagicSword : Skill
{
    [SerializeField] int turboRequired;

    public override void Act(ActionParameter actionParameter)
    {
        if (actionParameter.attacker.turboCounter < turboRequired) return;

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
        return Utility.CalculateDamage(attacker.currentAtk * 2 + attacker.currentMagic * 2, target.currentDef);
    }
}
