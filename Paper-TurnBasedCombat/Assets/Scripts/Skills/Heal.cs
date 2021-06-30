using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Skills/Heal")]
public class Heal : Skill
{
    public override void Act(ActionParameter actionParameter)
    {
        base.Act(actionParameter);

        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterAllyState();
                break;
            case TargetChoiceParameter p:
                p.target.currentHp = Mathf.Clamp(p.target.currentHp + p.attacker.currentMagic, 0, p.target.currentMaxHp);

                string dmgText = p.attacker.name + " heals " + p.target.name + " for " + p.attacker.currentMagic + ".";
                BattleSystem.instance.EnterDialogueState(dmgText);
                break;
        }
    }
}
