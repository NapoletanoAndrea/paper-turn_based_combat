using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueState { InQueue, Reading, Finished }

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogueText;
    public Dialogue nextDialogue;
    [HideInInspector] public DialogueState state;
}
