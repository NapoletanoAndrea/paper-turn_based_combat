using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Skills/Heal")]
public class Heal : Skill
{
    public override void Act(StatsHandler attacker, StatsHandler target)
    {
        target.currentHp = Mathf.Clamp(target.currentHp + attacker.currentMagic, 0, target.currentMaxHp);
        base.Act(attacker, target);
    }
}
