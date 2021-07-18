using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [HideInInspector] public List<string> dialogueQueue = new List<string>();
    [HideInInspector] public string currentDialogue;
    [SerializeField] float dialogueDelay;
    bool finishedDialogue;

    [SerializeField] string startDialogue;

    [SerializeField] List<StatsHandler> players = new List<StatsHandler>();
    [SerializeField] List<StatsHandler> enemies = new List<StatsHandler>();

    Dictionary<StatsHandler, Text> charactersNames;
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

    Dictionary<StatsHandler, List<GameObject>> skillsManaRequired = new Dictionary<StatsHandler, List<GameObject>>();
    Dictionary<StatsHandler, List<GameObject>> skillDescriptions = new Dictionary<StatsHandler, List<GameObject>>();
    GameObject activeSkillDescription;
    GameObject activeManaRequired;

    List<GameObject> actionCursors = new List<GameObject>();
    List<GameObject> skillCursors = new List<GameObject>();
    List<GameObject> allyTargetCursors = new List<GameObject>();
    List<GameObject> enemyTargetCursors = new List<GameObject>();

    GameObject activeCursor;

    private void Awake()
    {
        instance = this;

        dialogueQueue.Add(startDialogue);

        charactersNames = new Dictionary<StatsHandler, Text>();

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
        dialogueTemplate.gameObject.SetActive(true);
        activeTemplates.Add(dialogueTemplate.gameObject);
        StartCoroutine(ReadDialogue(dialogueQueue[0], dialogueDelay));
    }

    private void InitializeActionTemplate()
    {
        Transform alliesActions = chooseActionTemplate.Find("AlliesActions");
        Transform allyActionsPrefab = alliesActions.Find("AllyActions");
        Transform actionTextPrefab = allyActionsPrefab.Find("actionText");
        Transform alliesInfo = chooseActionTemplate.Find("AlliesInfo");
        Transform allyInfoPrefab = alliesInfo.Find("AllyInfo");

        actionCursors.Add(chooseActionTemplate.Find("cursor").gameObject);
        RectTransform actionCursorRect = actionCursors[0].GetComponent<RectTransform>();
        for (int i = 0; i < 3; i++)
        {
            RectTransform cursorRect = Instantiate(actionCursors[0], chooseActionTemplate).GetComponent<RectTransform>();
            cursorRect.anchoredPosition = new Vector2(actionCursorRect.anchoredPosition.x, actionCursorRect.anchoredPosition.y - (yDistance * (i + 1)));
            actionCursors.Add(cursorRect.gameObject);
        }

        for (int i = 0; i < players.Count; i++)
        {
            Transform allyInfo = Instantiate(allyInfoPrefab, alliesInfo);
            allyInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -yDistance);

            Text nameText = allyInfo.Find("Name").GetComponent<Text>();
            nameText.text = players[i].charInfo.name;
            charactersNames.Add(players[i], nameText);

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
                actionText.gameObject.SetActive(true);
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

        int x = 0;
        int y = 0;

        RectTransform actionCursorRect = actionCursors[0].GetComponent<RectTransform>();

        for (int i = 0; i < 8; i++)
        {
            RectTransform cursorRect = Instantiate(actionCursors[0], chooseSkillTemplate).GetComponent<RectTransform>();
            cursorRect.anchoredPosition = new Vector2(actionCursorRect.anchoredPosition.x, actionCursorRect.anchoredPosition.y - (yDistance * i));
            skillCursors.Add(cursorRect.gameObject);
        }

        foreach (StatsHandler player in players)
        {
            x = 0;
            y = 0;

            Transform allySkills = Instantiate(allySkillsPrefab, chooseSkillTemplate);
            charSkillsUI.Add(player, allySkills.gameObject);

            List<GameObject> descriptions = new List<GameObject>();
            List<GameObject> manaRequireds = new List<GameObject>();

            foreach(Skill skill in player.charInfo.skills)
            {
                Transform skillTransform = Instantiate(skillPrefab, allySkills);

                RectTransform nameRect = skillTransform.Find("Name").GetComponent<RectTransform>();
                nameRect.anchoredPosition = new Vector2(x * xDistance, y * -yDistance);
                nameRect.GetComponent<Text>().text = skill.actionName;

                GameObject manaRequired = skillTransform.Find("ManaRequired").gameObject;
                manaRequired.GetComponent<Text>().text = "Mana Required: " + skill.manaConsumed;
                manaRequired.SetActive(false);
                manaRequireds.Add(manaRequired);

                GameObject skillDescription = skillTransform.Find("Description").gameObject;
                skillDescription.GetComponent<Text>().text = skill.skillDescription;
                skillDescription.SetActive(false);
                descriptions.Add(skillDescription);

                skillTransform.gameObject.SetActive(true);

                y++;
                if(y == 4)
                {
                    x++;
                    y = 0;
                }
            }

            skillDescriptions.Add(player, descriptions);
            skillsManaRequired.Add(player, manaRequireds);
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
            currentHpText.text = enemies[i].currentMaxHp.ToString();

            Text maxHpText = hpInfo.Find("maxHpText").GetComponent<Text>();
            maxHpText.text = enemies[i].currentHp.ToString();

            Slider hpBar = hpInfo.Find("Healthbar").GetComponent<Slider>();
            hpBar.maxValue = enemies[i].currentMaxHp;
            hpBar.value = enemies[i].currentHp;

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

    public void HighlightName(StatsHandler character)
    {
        foreach(KeyValuePair<StatsHandler, Text> characterName in charactersNames)
        {
            characterName.Value.color = new Color(0.196f, 0.196f, 0.196f);
        }

        charactersNames[character].color = Color.red;
    }

    public void UpdateActionCursor(int index)
    {
        if (activeCursor)
            activeCursor.SetActive(false);

        actionCursors[index].SetActive(true);
        activeCursor = actionCursors[index];
    }

    public void UpdateEnemyCursor(int index)
    {
        if (activeCursor)
            activeCursor.SetActive(false);

        enemyTargetCursors[index].SetActive(true);
        activeCursor = enemyTargetCursors[index];
    }

    public void UpdateSkillCursor(StatsHandler player, int index)
    {
        activeCursor?.SetActive(false);

        skillCursors[index].SetActive(true);
        activeCursor = skillCursors[index];

        activeSkillDescription?.SetActive(false);

        activeSkillDescription = skillDescriptions[player][index];
        activeSkillDescription.SetActive(true);

        activeManaRequired?.SetActive(false);

        activeManaRequired = skillsManaRequired[player][index];
        activeManaRequired.SetActive(true);
    }


    public void UpdateAllyCursor(int index)
    {
        if (activeCursor)
            activeCursor.SetActive(false);

        allyTargetCursors[index].SetActive(true);
        activeCursor = allyTargetCursors[index];
    }

    public bool SkipDialogue()
    {
        switch (finishedDialogue)
        {
            case false:
                StopAllCoroutines();
                currentText.text = currentDialogue;
                dialogueQueue.RemoveAt(0);
                finishedDialogue = true;
                return false;
            case true:
                if (dialogueQueue.Count > 0)
                {
                    StartCoroutine(ReadDialogue(dialogueQueue[0], dialogueDelay));
                    return false;
                }
                break;          
        }

        return true;
    }

    private IEnumerator ReadDialogue(string dialogue, float waitSeconds)
    {
        currentText.text = "";
        currentDialogue = dialogue;
        finishedDialogue = false;

        for(int i = 0; i < dialogue.Length; i++)
        {
            currentText.text += dialogue[i];
            yield return new WaitForSeconds(waitSeconds);
        }

        dialogueQueue.RemoveAt(0);
        finishedDialogue = true;
    }

    public void SwitchToDialogueTemplate()
    {
        DeactivateTemplates();

        dialogueTemplate.gameObject.SetActive(true);
        activeTemplates.Add(dialogueTemplate.gameObject);
        StartCoroutine(ReadDialogue(dialogueQueue[0], dialogueDelay));
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

    public void SwitchToEnemyTemplate()
    {
        DeactivateTemplates();

        chooseEnemyTemplate.gameObject.SetActive(true);
        activeTemplates.Add(chooseEnemyTemplate.gameObject);
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
