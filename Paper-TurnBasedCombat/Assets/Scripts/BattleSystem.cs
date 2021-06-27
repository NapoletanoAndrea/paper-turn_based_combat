using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState { Dialogue, ActionChoice, SkillChoice, EnemyTargetChoice, AllyTargetChoice }

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;

    BattleState state;

    public List<StatsHandler> players;
    public List<StatsHandler> enemies;
    int charIndex;
    int enemyIndex;

    [HideInInspector] public List<StatsHandler> actors = new List<StatsHandler>();
    int actorIndex = -1;

    Action chosenAction;
    Skill chosenSkill;

    int actionIndex;

    private void Awake()
    {
        instance = this;
        state = BattleState.Dialogue;

        foreach (var p in players)
            actors.Add(p);
        foreach (var e in enemies)
            actors.Add(e);
    }

    private void Start()
    {
        actors = actors.OrderByDescending(a => a.currentSpeed).ToList();
    }

    private void Update()
    {
        switch (state)
        {
            case BattleState.Dialogue:
                if (Input.GetKeyDown(KeyCode.Return))
                    if (UIManager.instance.SkipDialogue())
                        UpdateTurn();
                break;
            case BattleState.ActionChoice:
                ChooseAction();
                break;
            case BattleState.SkillChoice:
                break;
            case BattleState.EnemyTargetChoice:
                ChooseEnemy();
                break;
        }
    }

    private void UpdateTurn()
    {
        actorIndex = Utility.IncrementInt(actorIndex + 1, players.Count);

        if (actors[actorIndex].CompareTag("Player"))
            EnterActionState();
    }

    public void EnterDialogueState(params string[] dialogues)
    {
        state = BattleState.Dialogue;

        foreach(string dialogue in dialogues)
            UIManager.instance.dialogueQueue.Add(dialogue);

        UIManager.instance.SwitchToDialogueTemplate();
    }

    private void EnterActionState()
    {
        state = BattleState.ActionChoice;
        UIManager.instance.HighlightName(actors[actorIndex]);
        UIManager.instance.SwitchToActionTemplate(actors[actorIndex]);
        UIManager.instance.UpdateActionCursor(0);
    }

    public void EnterEnemyTargetState()
    {
        state = BattleState.EnemyTargetChoice;
        UIManager.instance.UpdateEnemyCursor(0);
        UIManager.instance.SwitchToEnemyTemplate();
    }

    private void ChooseAction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            actionIndex = Utility.IncrementInt(actionIndex - 1, 4);
            UIManager.instance.UpdateActionCursor(actionIndex);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            actionIndex = Utility.IncrementInt(actionIndex + 1, 4);
            UIManager.instance.UpdateActionCursor(actionIndex);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chosenAction = players[charIndex].GetAction(actionIndex);
            chosenAction.Act(new ActionChoiceParameter(players[charIndex]));
        }
    }

    private void ChooseEnemy()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            enemyIndex = Utility.IncrementInt(enemyIndex - 1, 4);
            UIManager.instance.UpdateEnemyCursor(enemyIndex);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            enemyIndex = Utility.IncrementInt(enemyIndex + 1, 4);
            UIManager.instance.UpdateEnemyCursor(enemyIndex);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chosenAction.Act(new TargetChoiceParameter(players[charIndex], enemies[enemyIndex]));
        }
    }

    private void ChooseSkill()
    {

    }
}
