using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/Inspire")]

public class Inspire : Skill
{
    [SerializeField] int duration;
    [SerializeField] Stat[] buffedStats;
    [SerializeField] int buff;

    public override void Act(ActionParameter actionParameter)
    {
        base.Act(actionParameter);

        foreach(var p in BattleSystem.instance.players)
        {
            if (!p.IsDead())
            {
                foreach(var stat in buffedStats)
                    new StatModification(duration, stat, buff).InflictStatus(p);
            }
        }

        BattleSystem.instance.EnterDialogueState(actionParameter.attacker.name + " has inspired the team");
    }
}
