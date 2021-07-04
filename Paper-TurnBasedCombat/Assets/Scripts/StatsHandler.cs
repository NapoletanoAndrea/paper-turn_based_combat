using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHandler : MonoBehaviour
{
    public event Action<Vector2, int> OnDamageTaken;

    public CharacterInfo charInfo;

    public int currentMaxHp;
    public int currentHp;
    public int currentAtk;
    public int currentDef;
    public int currentMagic;
    public int currentMaxMana;
    public int currentMana;
    public int currentSpeed;

    SpriteRenderer sR;

    public List<Status> inflictedStatuses = new List<Status>();

    private void Awake()
    {
        currentMaxHp = charInfo.maxHp;
        currentHp = charInfo.maxHp;
        currentAtk = charInfo.baseAtk;
        currentDef = charInfo.baseDef;
        currentMagic = charInfo.baseMagic;
        currentMaxMana = charInfo.maxMana;
        currentMana = charInfo.maxMana;
        currentSpeed = charInfo.speed;

        sR = GetComponent<SpriteRenderer>();
    }

    public Action GetAction(int index)
    {
        return charInfo.actions[index];
    }

    public Skill GetSkill(int index)
    {
        return charInfo.skills[index];
    }

    public int GetSkillNum()
    {
        return charInfo.skills.Count;
    }

    public void UpdateStatus()
    {
        foreach (var status in inflictedStatuses)
            status.UpdateStatus(this);
    }

    public void TakeDamage(int damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, currentMaxHp);
        UIManager.instance.UpdateHp(this);
        OnDamageTaken?.Invoke(transform.position, damage);

        if (currentHp == 0) Die();
        else sR.color = Color.white;
    }

    private void Die()
    {
        inflictedStatuses.Clear();
        string dialogue = name + " has died";
        UIManager.instance.dialogueQueue.Add(dialogue);
        sR.color = new Color(0.8301887f, 0.09006765f, 0.09006765f);
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }
}
