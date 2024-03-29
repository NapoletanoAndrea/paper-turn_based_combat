﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/Heal")]
public class Heal : Skill
{
    [SerializeField] float healMultiplier = 1;

    public override void Act(ActionParameter actionParameter)
    {
        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterAllyState();
                break;
            case TargetChoiceParameter p:
                if (p.target.IsDead()) return;
                base.Act(actionParameter);
                p.target.TakeDamage((int)-(p.attacker.currentMagic * healMultiplier));
                
                string healTarget = p.attacker == p.target ? "self" : p.target.name;
                string dmgText = p.attacker.name + " heals " + healTarget + " for " + p.attacker.currentMagic * healMultiplier + ".";
                BattleSystem.instance.EnterDialogueState(dmgText);
                break;
        }
    }
}
