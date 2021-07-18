using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/GroupHeal")]
public class GroupHeal : Skill
{
    [SerializeField] float healMultiplier = 1;

    public override void Act(ActionParameter actionParameter)
    {
        base.Act(actionParameter);

        foreach(var p in BattleSystem.instance.players)
        {
            if(!p.IsDead())
                p.TakeDamage((int)-(actionParameter.attacker.currentMagic * healMultiplier));
        }

        string text = "The entire party is healed by " + (int)(actionParameter.attacker.currentMagic * healMultiplier);
        BattleSystem.instance.EnterDialogueState(text);
    }
}
