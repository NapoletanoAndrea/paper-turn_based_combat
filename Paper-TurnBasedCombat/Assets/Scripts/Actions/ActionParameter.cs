public abstract class ActionParameter
{
    public StatsHandler attacker;
}

public class ActionChoiceParameter : ActionParameter
{
    public ActionChoiceParameter(StatsHandler attacker)
    {
        this.attacker = attacker;
    }
}

public class TargetChoiceParameter : ActionParameter
{
    public StatsHandler target;

    public TargetChoiceParameter(StatsHandler attacker, StatsHandler target)
    {
        this.attacker = attacker;
        this.target = target;
    }
}
