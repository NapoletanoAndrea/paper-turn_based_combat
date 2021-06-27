using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "ScriptableObjects/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    public string characterName;

    public int maxHp;
    public int baseAtk;
    public int baseDef;
    public int baseMagic;
    public int maxMana;
    public int speed;

    public List<Action> actions;

    public List<Skill> skills;
}
