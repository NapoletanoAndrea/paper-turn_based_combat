using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHandler : MonoBehaviour
{
    public CharacterInfo charInfo;

    public int currentMaxHp;
    public int currentHp;
    public int currentAtk;
    public int currentDef;
    public int currentMagic;
    public int currentMaxMana;
    public int currentMana;

    private void Awake()
    {
        currentMaxHp = charInfo.maxHp;
        currentHp = charInfo.maxHp;
        currentAtk = charInfo.baseAtk;
        currentDef = charInfo.baseDef;
        currentMagic = charInfo.baseMagic;
        currentMaxMana = charInfo.maxMana;
        currentMana = charInfo.maxMana;
    }

    public void TakeDamage(int damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, currentMaxHp);
        if (currentHp == 0)
            Die();
    }

    private void Die()
    {
        Debug.Log(charInfo.name + " has died.");
    }
}
