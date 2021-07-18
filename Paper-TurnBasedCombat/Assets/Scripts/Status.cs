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

    public virtual bool InflictStatus(StatsHandler target)
    {
        foreach(var status in target.inflictedStatuses)
        {
            if (status.GetType() == GetType())
                return false;
        }
        target.inflictedStatuses.Add(this);
        return true;
    }

    public virtual bool UpdateStatusBefore(StatsHandler target) { return true; }

    public virtual bool UpdateStatus(StatsHandler target)
    {
        passedTurns++;
        if (passedTurns >= duration)
        {
            OnStatusRemoved(target);
            target.inflictedStatuses.Remove(this);
            return false;
        }
        return true;
    }

    public virtual void OnStatusRemoved(StatsHandler target) { }
}

public class HasTaunted : Status
{
    public HasTaunted(int duration) : base(duration) { }
    public override bool UpdateStatus(StatsHandler target)
    {
        if (!base.UpdateStatus(target))
        {
            UIManager.instance.dialogueQueue.Add(target.name + " is not the target of enemy attacks anymore");
            return false;
        }
        return true;
    }

}

public class Poison : Status
{
    public Poison(int duration) : base(duration) { }

    public override bool UpdateStatus(StatsHandler target)
    {
        target.TakeDamage((int)(target.currentMaxHp * .1f));
        UIManager.instance.dialogueQueue.Add(target.name + " takes " + (int)(target.currentMaxHp * .1f) + " poison damage");

        passedTurns++;
        if (passedTurns >= duration)
        {
            OnStatusRemoved(target);
            UIManager.instance.dialogueQueue.Add("The poison effect on " + target.name + " has worn off");
            target.inflictedStatuses.Remove(this);
            return false;
        }
        return true;
    }
}

public class StatModification : Status
{
    Stat stat;
    int value;

    public StatModification(int duration, Stat stat, int value) : base(duration) 
    {
        this.stat = stat;
        this.value = value;
    }

    public override bool InflictStatus(StatsHandler target)
    {
        foreach (var status in target.inflictedStatuses)
        {
            if (status.GetType() == GetType())
            {
                if(((StatModification)status).stat == stat)
                {
                    status.passedTurns = duration;
                    UpdateStatus(target);
                    break;
                }
            }                
        }

        target.inflictedStatuses.Add(this);
        Utility.GetStat(target, stat) += value;
        return true;
    }

    public override void OnStatusRemoved(StatsHandler target)
    {
        Utility.GetStat(target, stat) -= value;
    }
}


public class Paralyzed : Status
{
    float paraChance;

    public Paralyzed(int duration, float paraChance) : base(duration)
    {
        this.paraChance = paraChance;
    }

    public override bool UpdateStatusBefore(StatsHandler target)
    {
        if (Random.Range(0f, 100f) <= paraChance)
            BattleSystem.instance.EnterDialogueState(target.name + " is paralyzed and unable to move");

        return true;
    }

    public override bool UpdateStatus(StatsHandler target)
    {
        passedTurns++;
        if (passedTurns >= duration)
        {
            OnStatusRemoved(target);
            UIManager.instance.dialogueQueue.Add("The paralyze effect on " + target.name + " has worn off");
            target.inflictedStatuses.Remove(this);
            return false;
        }

        return true;
    }
}