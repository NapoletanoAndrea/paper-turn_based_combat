using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Basic Actions/ModifyStat")]
public class ModifyStat : Action
{
    [SerializeField] Stat stat;
    [SerializeField] int value;
    [SerializeField] int duration;

    public override void Act(ActionParameter actionParameter)
    {
        new StatModification(duration, stat, value).InflictStatus(actionParameter.attacker);
        BattleSystem.instance.EnterDialogueState(actionParameter.attacker.name + " is now guarding.");
    }
}
