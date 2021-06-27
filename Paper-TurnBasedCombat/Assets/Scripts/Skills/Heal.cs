using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Skills/Heal")]
public class Heal : Skill
{
    public override void Act(ActionParameter actionParameter)
    {
        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                
                break;
            case TargetChoiceParameter p:
                p.target.currentHp = Mathf.Clamp(p.target.currentHp + p.attacker.currentMagic, 0, p.target.currentMaxHp);
                base.Act(p);
                break;
        }
    }
}
