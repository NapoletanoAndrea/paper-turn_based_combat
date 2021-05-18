using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    int index = 0;

    [SerializeField] StatsHandler[] players;
    Dictionary<StatsHandler, StatUI> charactersHp;
    Dictionary<StatsHandler, StatUI> charactersMana;

    [SerializeField] float xDistance;
    [SerializeField] float yDistance;

    Transform chooseActionTemplate;
    Transform dialogueTemplate;
    Transform chooseSkillTemplate;
    Transform chooseEnemyTemplate; 

    private void Awake()
    {
        instance = this;

        charactersHp = new Dictionary<StatsHandler, StatUI>();
        charactersMana = new Dictionary<StatsHandler, StatUI>();

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
        Transform alliesActions = chooseActionTemplate.Find("AlliesActions");
        Transform allyActionsPrefab = alliesActions.Find("AllyActions");
        Transform actionTextPrefab = allyActionsPrefab.Find("actionText");
        Transform alliesInfo = chooseActionTemplate.Find("AlliesInfo");
        Transform allyInfoPrefab = alliesInfo.Find("AllyInfo");

        for(int i = 0; i < players.Length; i++)
        {
            Transform allyInfo = Instantiate(allyInfoPrefab, alliesInfo);
            allyInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -yDistance);
            allyInfo.Find("Name").GetComponent<Text>().text = players[i].charInfo.name;

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

            int x = 0;
            int y = 0;

            foreach (Action action in players[i].charInfo.actions)
            {                
                RectTransform actionText = Instantiate(actionTextPrefab, allyActions).GetComponent<RectTransform>();
                actionText.GetComponent<Text>().text = action.actionName;
                actionText.anchoredPosition = new Vector2(x * xDistance, y * -yDistance);
                y++;

                if(y == 4)
                {
                    x++;
                    y = 0;
                }
            }

            allyActions.gameObject.SetActive(true);
            allyInfo.gameObject.SetActive(true);
        }
    }

    public void UpdateHp(StatsHandler character)
    {
        charactersHp[character].UpdateUI(character.currentMaxHp, character.currentHp);
    }

    public void UpdateMana(StatsHandler character)
    {
        charactersMana[character].UpdateUI(character.currentMaxMana, character.currentMaxMana);
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
