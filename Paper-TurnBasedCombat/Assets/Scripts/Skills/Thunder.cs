using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/Thunder")]
public class Thunder : Skill
{
    [SerializeField] Stat attackStat;
    [SerializeField] float dmgMultiplier;
    [SerializeField] float paraChance;
    [SerializeField] int paraDuration;

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
                if (new Paralyzed(paraDuration, paraChance).InflictStatus(p.target))
                    UIManager.instance.dialogueQueue.Add(p.target.name + " has been paralyzed");
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