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

    [HideInInspector] public List<StatsHandler> actors = new List<StatsHandler>();
    int actorIndex = -1;

    Action chosenAction;

    int index;

    [SerializeField] GameObject damageText;

    private void Awake()
    {
        instance = this;
        state = BattleState.Dialogue;

        foreach (var p in players)
        {
            actors.Add(p);
            p.OnDamageTaken += Handler_OnDamageTaken;
        }
        foreach (var e in enemies)
        {
            actors.Add(e);
            e.OnDamageTaken += Handler_OnDamageTaken;
        }
    }

    private void Handler_OnDamageTaken(Vector2 position, int damage)
    {
        Vector2 startPosition = position /*+ Vector2.right * -.6f*/;
        Vector2 endPoint = startPosition + Vector2.up * .8f;
        Instantiate(damageText, startPosition, Quaternion.identity).GetComponent<DamageTextBehaviour>().Init(damage, endPoint);
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
                ChooseSkill();
                break;
            case BattleState.EnemyTargetChoice:
                ChooseEnemy();
                break;
            case BattleState.AllyTargetChoice:
                ChooseAlly();
                break;
        }
    }

    private void UpdateTurn()
    {
        if(actorIndex >= 0) actors[actorIndex].UpdateStatus();
        do
        {
            actorIndex = Utility.IncrementInt(actorIndex + 1, actors.Count);
        }
        while (actors[actorIndex].IsDead());

        actors[actorIndex].UpdateStatusBefore();

        if (actors[actorIndex].CompareTag("Player"))
            EnterActionState();
        else
            actors[actorIndex].GetComponent<ScoreController>().DoAction(actors[actorIndex]);
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
        index = 0;
    }

    public void EnterEnemyTargetState()
    {
        state = BattleState.EnemyTargetChoice;
        UIManager.instance.UpdateEnemyCursor(0);
        UIManager.instance.SwitchToEnemyTemplate();
        index = 0;
    }

    public void EnterSkillState()
    {
        state = BattleState.SkillChoice;
        UIManager.instance.UpdateSkillCursor(actors[actorIndex], 0);
        UIManager.instance.SwitchToSkillTemplate(actors[actorIndex]);
        index = 0;
    }

    public void EnterAllyState()
    {
        state = BattleState.AllyTargetChoice;
        UIManager.instance.UpdateAllyCursor(0);
        UIManager.instance.SwitchToActionTemplate(actors[actorIndex]);
        index = 0;
    }

    private void ChooseAction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = Utility.IncrementInt(index - 1, 4);
            UIManager.instance.UpdateActionCursor(index);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = Utility.IncrementInt(index + 1, 4);
            UIManager.instance.UpdateActionCursor(index);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chosenAction = actors[actorIndex].GetAction(index);
            chosenAction.Act(new ActionChoiceParameter(actors[actorIndex]));
        }
    }

    private void ChooseEnemy()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = Utility.IncrementInt(index - 1, enemies.Count);
            UIManager.instance.UpdateEnemyCursor(index);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = Utility.IncrementInt(index + 1, enemies.Count);
            UIManager.instance.UpdateEnemyCursor(index);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chosenAction.Act(new TargetChoiceParameter(actors[actorIndex], enemies[index]));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnterActionState();
        }
    }

    private void ChooseSkill()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = Utility.IncrementInt(index - 1, actors[actorIndex].GetSkillNum());
            UIManager.instance.UpdateSkillCursor(actors[actorIndex], index);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = Utility.IncrementInt(index + 1, actors[actorIndex].GetSkillNum());
            UIManager.instance.UpdateSkillCursor(actors[actorIndex], index);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chosenAction = actors[actorIndex].GetSkill(index);
            if (actors[actorIndex].currentMana < ((Skill)chosenAction).manaConsumed) return;
            chosenAction.Act(new ActionChoiceParameter(actors[actorIndex]));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnterActionState();
        }
    }

    private void ChooseAlly()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = Utility.IncrementInt(index - 1, players.Count);
            UIManager.instance.UpdateAllyCursor(index);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = Utility.IncrementInt(index + 1, players.Count);
            UIManager.instance.UpdateAllyCursor(index);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chosenAction.Act(new TargetChoiceParameter(actors[actorIndex], players[index]));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnterActionState();
        }
    }

    public StatsHandler GetRandomTarget()
    {
        StatsHandler target;

        do
        {
            target = players[UnityEngine.Random.Range(0, players.Count)];
        }
        while (target.IsDead());

        return target;
    }

    public StatsHandler GetCurrentActor()
    {
        return actors[actorIndex];
    }
}
