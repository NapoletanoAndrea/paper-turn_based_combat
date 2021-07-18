using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/StrongAttack")]

public class StrongAttack : Skill
{
    [SerializeField] Stat attackStat;
    [SerializeField] float dmgMultiplier;

    public override void Act(ActionParameter actionParameter)
    {
        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterEnemyTargetState();
                break;
            case TargetChoiceParameter p:
                base.Act(actionParameter);
                int damage = GetDamage(p.attacker, p.target);
                p.target.TakeDamage(damage);

                string text = p.attacker.name + " uses " + name + " on " + p.target.name + ".";
                string dmgText = p.attacker.name + " deals " + damage + "  damage to " + p.target.name + ".";
                BattleSystem.instance.EnterDialogueState(text, dmgText);
                break;
        }
    }

    public override int GetDamage(StatsHandler attacker, StatsHandler target)
    {
        return Utility.CalculateDamage(GetAttackStat(attacker), target.currentDef);
    }

    public override int GetAttackStat(StatsHandler attacker)
    {
        return Utility.GetStat(attacker, attackStat);
    }
}
