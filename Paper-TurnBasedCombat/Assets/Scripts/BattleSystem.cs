using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Dialogue, ActionChoice, TargetChoice }

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;

    BattleState state;

    [SerializeField] List<StatsHandler> players = new List<StatsHandler>();

    int actionIndex;

    private void Awake()
    {
        instance = this;
        state = BattleState.Dialogue; 
    }

    private void Update()
    {
        switch (state)
        {
            case BattleState.Dialogue:
                if(Input.GetKeyDown(KeyCode.Return))
                    if (UIManager.instance.SkipDialogue())
                        state = BattleState.ActionChoice;
                break;
            case BattleState.ActionChoice:
                ChooseAction();
                break;
        }
    }

    private void ChooseAction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            actionIndex = Mathf.Clamp(actionIndex - 1, 0, 3);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {

        }
    }
}
