using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/PoisonHeal")]
public class PoisonHeal : Skill
{
    public override void Act(ActionParameter actionParameter)
    {
        switch (actionParameter)
        {
            case ActionChoiceParameter p:
                BattleSystem.instance.EnterAllyState();
                break;
            case TargetChoiceParameter p:
                base.Act(actionParameter);
                string text = p.target.name + " is not poisoned.";

                foreach (var status in p.target.inflictedStatuses)
                {
                    if (status is Poison)
                    {
                        status.OnStatusRemoved(p.target);
                        p.target.inflictedStatuses.Remove(status);
                        text = p.target.name + " has been cured from poison.";
                    }
                }

                BattleSystem.instance.EnterDialogueState(text);
                break;
        }
    }
}
