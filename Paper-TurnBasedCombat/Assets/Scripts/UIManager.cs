using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Dialogue startDialogue;
    [SerializeField] float dialogueDelay;
    [HideInInspector] public Dialogue currentDialogue;

    [SerializeField] List<StatsHandler> players = new List<StatsHandler>();
    [SerializeField] List<StatsHandler> enemies = new List<StatsHandler>();
    Dictionary<StatsHandler, StatUI> charactersHp;
    Dictionary<StatsHandler, StatUI> charactersMana;
    Dictionary<StatsHandler, GameObject> charActionsUI;
    Dictionary<StatsHandler, GameObject> charSkillsUI;

    [SerializeField] float xDistance;
    [SerializeField] float yDistance;

    Transform chooseActionTemplate;
    Transform dialogueTemplate;
    Transform chooseSkillTemplate;
    Transform chooseEnemyTemplate;

    List<GameObject> activeTemplates = new List<GameObject>();

    Text currentText;

    List<GameObject> actionCursors = new List<GameObject>();
    List<GameObject> skillCursors = new List<GameObject>();
    List<GameObject> allyTargetCursors = new List<GameObject>();
    List<GameObject> enemyTargetCursors = new List<GameObject>();

    GameObject activeCursor;

    private void Awake()
    {
        instance = this;

        charactersHp = new Dictionary<StatsHandler, StatUI>();
        charactersMana = new Dictionary<StatsHandler, StatUI>();
        charActionsUI = new Dictionary<StatsHandler, GameObject>();
        charSkillsUI = new Dictionary<StatsHandler, GameObject>();

        chooseActionTemplate = transform.Find("ChooseActionTemplate");
        dialogueTemplate = transform.Find("DialogueTemplate");
        chooseSkillTemplate = transform.Find("ChooseSkillTemplate");
        chooseEnemyTemplate = transform.Find("ChooseEnemyTemplate");
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        InitializeDialogueTemplate();
        InitializeActionTemplate();
        InitializeSkillTemplate();
        InitializeEnemyTargetTemplate();
    }

    private void InitializeDialogueTemplate()
    {
        currentText = dialogueTemplate.Find("DialogueText").GetComponent<Text>();
        activeTemplates.Add(dialogueTemplate.gameObject);
        StartCoroutine(ReadDialogue(startDialogue, dialogueDelay));
    }

    private void InitializeActionTemplate()
    {
        Transform alliesActions = chooseActionTemplate.Find("AlliesActions");
        Transform allyActionsPrefab = alliesActions.Find("AllyActions");
        Transform actionTextPrefab = allyActionsPrefab.Find("actionText");
        Transform alliesInfo = chooseActionTemplate.Find("AlliesInfo");
        Transform allyInfoPrefab = alliesInfo.Find("AllyInfo");

        for (int i = 0; i < players.Count; i++)
        {
            Transform allyInfo = Instantiate(allyInfoPrefab, alliesInfo);
            allyInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -yDistance);
            allyInfo.Find("Name").GetComponent<Text>().text = players[i].charInfo.name;
            allyTargetCursors.Add(allyInfo.Find("cursor").gameObject);

            #region InitHp

            Transform hpInfo = allyInfo.Find("HpInfo");

            Text currentHpText = hpInfo.Find("currentHpText").GetComponent<Text>();
            currentHpText.text = players[i].currentMaxHp.ToString();

            Text maxHpText = hpInfo.Find("maxHpText").GetComponent<Text>();
            maxHpText.text = players[i].currentHp.ToString();

            Slider hpBar = hpInfo.Find("Healthbar").GetComponent<Slider>();
            hpBar.maxValue = players[i].currentMaxHp;
            hpBar.value = players[i].currentHp;

            charactersHp.Add(players[i], new StatUI(hpBar, currentHpText, maxHpText));

            #endregion

            #region InitMana

            Transform manaInfo = allyInfo.Find("ManaInfo");

            Text currentManaText = manaInfo.Find("currentManaText").GetComponent<Text>();
            currentManaText.text = players[i].currentMaxMana.ToString();

            Text maxManaText = manaInfo.Find("maxManaText").GetComponent<Text>();
            maxManaText.text = players[i].currentMana.ToString();

            Slider manaBar = manaInfo.Find("manabar").GetComponent<Slider>();
            manaBar.maxValue = players[i].currentMaxMana;
            manaBar.value = players[i].currentMana;

            charactersMana.Add(players[i], new StatUI(manaBar, currentManaText, maxManaText));

            #endregion

            Transform allyActions = Instantiate(allyActionsPrefab, alliesActions);
            charActionsUI.Add(players[i], allyActions.gameObject);

            int x = 0;
            int y = 0;

            foreach (Action action in players[i].charInfo.actions)
            {
                RectTransform actionText = Instantiate(actionTextPrefab, allyActions).GetComponent<RectTransform>();
                actionText.GetComponent<Text>().text = action.actionName;
                actionText.anchoredPosition = new Vector2(x * xDistance, y * -yDistance);
                actionCursors.Add(actionText.transform.Find("cursor").gameObject);
                y++;

                if (y == 4)
                {
                    x++;
                    y = 0;
                }
            }

            allyInfo.gameObject.SetActive(true);
        }
    }

    private void InitializeSkillTemplate()
    {
        Transform allySkillsPrefab = chooseSkillTemplate.Find("AllySkills");
        Transform skillPrefab = allySkillsPrefab.Find("Skill");

        foreach(StatsHandler player in players)
        {
            Transform allySkills = Instantiate(allySkillsPrefab, chooseSkillTemplate);
            charSkillsUI.Add(player, allySkills.gameObject);

            int x = 0;
            int y = 0;

            foreach(Skill skill in player.charInfo.skills)
            {
                Transform skillTransform = Instantiate(skillPrefab, allySkills);

                RectTransform nameRect = skillTransform.Find("Name").GetComponent<RectTransform>();
                nameRect.anchoredPosition = new Vector2(x * xDistance, y * -yDistance);
                nameRect.GetComponent<Text>().text = skill.name;

                skillTransform.Find("Description").GetComponent<Text>().text = skill.skillDescription;

                RectTransform skillCursor = skillTransform.Find("cursor").GetComponent<RectTransform>();
                skillCursor.anchoredPosition = new Vector2(skillCursor.anchoredPosition.x + x * xDistance, nameRect.anchoredPosition.y);
                skillCursors.Add(skillCursor.gameObject);

                skillTransform.gameObject.SetActive(true);

                y++;
                if(y == 4)
                {
                    x++;
                    y = 0;
                }
            }
        }
    }

    private void InitializeEnemyTargetTemplate()
    {
        Transform enemyInfoPrefab = chooseEnemyTemplate.Find("EnemyInfo");
        
        for(int i = 0; i < enemies.Count; i++)
        {
            Transform enemyInfo = Instantiate(enemyInfoPrefab, chooseEnemyTemplate);
            enemyInfo.Find("Name").GetComponent<Text>().text = enemies[i].charInfo.name;
            enemyTargetCursors.Add(enemyInfo.Find("cursor").gameObject);

            Transform hpInfo = enemyInfo.Find("HpInfo");

            Text currentHpText = hpInfo.Find("currentHpText").GetComponent<Text>();
            currentHpText.text = players[i].currentMaxHp.ToString();

            Text maxHpText = hpInfo.Find("maxHpText").GetComponent<Text>();
            maxHpText.text = players[i].currentHp.ToString();

            Slider hpBar = hpInfo.Find("Healthbar").GetComponent<Slider>();
            hpBar.maxValue = players[i].currentMaxHp;
            hpBar.value = players[i].currentHp;

            charactersHp.Add(enemies[i], new StatUI(hpBar, currentHpText, maxHpText));

            enemyInfo.gameObject.SetActive(true);
        }
    }

    private void DeactivateTemplates()
    {
        foreach (GameObject template in activeTemplates)
            template.SetActive(false);

        activeTemplates.Clear();
    }

    public void UpdateHp(StatsHandler character)
    {
        charactersHp[character].UpdateUI(character.currentMaxHp, character.currentHp);
    }

    public void UpdateMana(StatsHandler character)
    {
        charactersMana[character].UpdateUI(character.currentMaxMana, character.currentMaxMana);
    }

    public void UpdateActionCursor(int index)
    {
        if (!activeCursor)
            activeCursor.SetActive(false);

        actionCursors[index].SetActive(true);
        activeCursor = actionCursors[index];
    }

    public bool SkipDialogue()
    {
        switch (currentDialogue.state)
        {
            case DialogueState.Reading:
                StopAllCoroutines();
                currentText.text = currentDialogue.dialogueText;
                currentDialogue.state = DialogueState.Finished;
                return false;
            case DialogueState.Finished:
                if (currentDialogue.nextDialogue != null)
                {
                    StartCoroutine(ReadDialogue(currentDialogue.nextDialogue, dialogueDelay));
                    return false;
                }
                break;
            default: break;               
        }

        currentDialogue.state = DialogueState.InQueue;
        return true;
    }

    private IEnumerator ReadDialogue(Dialogue dialogue, float waitSeconds)
    {
        currentText.text = "";
        currentDialogue = dialogue;
        currentDialogue.state = DialogueState.Reading;

        for(int i = 0; i < dialogue.dialogueText.Length; i++)
        {
            currentText.text += dialogue.dialogueText[i];
            yield return new WaitForSeconds(waitSeconds);
        }

        currentDialogue.state = DialogueState.Finished;
    }

    public void SwitchToActionTemplate(StatsHandler player)
    {
        DeactivateTemplates();

        chooseActionTemplate.gameObject.SetActive(true);
        activeTemplates.Add(chooseActionTemplate.gameObject);

        charActionsUI[player].SetActive(true);
        activeTemplates.Add(charActionsUI[player]);
    }

    public void SwitchToSkillTemplate(StatsHandler player)
    {
        DeactivateTemplates();

        chooseSkillTemplate.gameObject.SetActive(true);
        activeTemplates.Add(chooseSkillTemplate.gameObject);

        charSkillsUI[player].SetActive(true);
        activeTemplates.Add(charSkillsUI[player]);
    }
}

public class StatUI
{
    Slider statBar;
    Text currentStatValueText;
    Text maxStatValueText;

    public StatUI(Slider statBar, Text currentStatValueText, Text maxStatValueText)
    {
        this.statBar = statBar;
        this.currentStatValueText = currentStatValueText;
        this.maxStatValueText = maxStatValueText;
    }

    public void UpdateUI(int maxValue, int currentValue)
    {
        statBar.maxValue = maxValue;
        statBar.value = currentValue;

        maxStatValueText.text = maxValue.ToString();
        currentStatValueText.text = currentValue.ToString();
    }
}
