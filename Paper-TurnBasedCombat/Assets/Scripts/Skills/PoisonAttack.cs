using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/PoisonAttack")]
public class PoisonAttack : Skill
{
    [SerializeField] Stat attackStat;
    [SerializeField] float dmgMultiplier;
    [SerializeField] float poisonChance;
    [SerializeField] int poisonDuration;

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
                if (Random.Range(0f, 100f) <= poisonChance)
                    if (new Poison(poisonDuration).InflictStatus(p.target))
                        UIManager.instance.dialogueQueue.Add(p.target.name + " has been poisoned");
                break;
        }
    }

    public override int GetDamage(StatsHandler attacker, StatsHandler target)
    {
        return Utility.CalculateDamage((int)(GetAttackStat(attacker) * dmgMultiplier), target.currentDef);
    }

    public override int GetAttackStat(StatsHandler attacker)
    {
        return Utility.GetStat(attacker, attackStat);
    }
}