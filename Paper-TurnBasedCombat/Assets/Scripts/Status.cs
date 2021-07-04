using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status
{
    public int duration;
    public int passedTurns;

    public Status(int duration)
    {
        this.duration = duration;
        passedTurns = 0;
    }

    public bool InflictStatus(StatsHandler target)
    {
        foreach(var status in target.inflictedStatuses)
        {
            if (status.GetType() == GetType())
                return false;
        }
        target.inflictedStatuses.Add(this);
        return true;
    }

    public virtual void UpdateStatus(StatsHandler target)
    {
        passedTurns++;
        if (passedTurns >= duration)
            target.inflictedStatuses.Remove(this);
    }
}

public class HasTaunted : Status
{
    public HasTaunted(int duration) : base(duration) { }
    public override void UpdateStatus(StatsHandler target)
    {
        base.UpdateStatus(target);
        UIManager.instance.dialogueQueue.Add(target.name + " is not the target of enemy attacks anymore");
    }

}

public class Poison : Status
{
    public Poison(int duration) : base(duration) { }

    public override void UpdateStatus(StatsHandler target)
    {
        target.TakeDamage((int)(target.currentMaxHp * .05f));
        base.UpdateStatus(target);
        UIManager.instance.dialogueQueue.Add("The poison effect on " + target.name + " has worn off");
    }
}
