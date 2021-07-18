using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/Resuscitate")]
public class Resuscitate : Skill
{
    [SerializeField] int turboRequired;

    public override void Act(ActionParameter actionParameter)
    {
        if (actionParameter.attacker.turboCounter < turboRequired) return;

        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterAllyState();
                break;
            case TargetChoiceParameter p:
                if (!p.target.IsDead()) return;
                base.Act(actionParameter);
                p.target.TakeDamage(-(p.target.currentMaxHp / 2));

                string text = p.target.name + " has come back to life";
                BattleSystem.instance.EnterDialogueState(text);
                break;
        }
    }
}
