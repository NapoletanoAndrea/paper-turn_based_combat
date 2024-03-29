﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "ScriptableObjects/Basic Actions/Skills")]
public class Skills : Action
{
    public override void Act(ActionParameter actionParameter)
    {
        BattleSystem.instance.EnterSkillState();
    }
}
