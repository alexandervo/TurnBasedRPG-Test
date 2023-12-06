using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class BattleStateMachine : MonoBehaviour
{
    
    public enum PerformAction
    {
        START,
        WAIT,
        //WAITFORINPUT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }

    public PerformAction battleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn> ();

    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject> ();

    public List<GameObject> Actors = new List<GameObject> ();
   
    //Turn things
    public int ChoicesMade = 0;
    public int CurrentTurn = 1;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text countdownText;
    private float currentCount = 0f;
    public float startingCount = 20f;

    //autobattle?
    public bool autoBattle = false;
    public int abTurns;
    public int abTurnsR;
    [SerializeField] private TMP_Text autoText;

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1, //select attack
        INPUT2, //select target
        DONE
    }

    public HeroGUI HeroInput;

    public List<GameObject> HeroesToManage = new List<GameObject> ();
    private HandleTurn HeroChoise;

    public GameObject enemyButton;
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    public GameObject MagicPanel;


    //magic attack
    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject magicButton;
    private List<GameObject> atkBtns = new List<GameObject> ();

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject> ();

    //spawnpoints
    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> heroSpawnPoints = new List<Transform>();

    void Awake()
    {
        for (int i = 0; i < GameManager.instance.enemyAmount; i++)
        {
            GameObject NewEnemy = Instantiate(GameManager.instance.enemysToBattle[i], spawnPoints[i].position, Quaternion.identity) as GameObject;
            NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName + " " + (i+1);
            NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName = NewEnemy.name;
            EnemysInBattle.Add(NewEnemy);
        }

        for (int i = 0; i < GameManager.instance.heroAmount; i++)
        {
            GameObject NewHero = Instantiate(GameManager.instance.battleHeroes[i], heroSpawnPoints[i].position, Quaternion.identity) as GameObject;
            NewHero.name = NewHero.GetComponent<HeroStateMachine>().hero.theName;
            NewHero.GetComponent<HeroStateMachine>().hero.theName = NewHero.name;
            HerosInBattle.Add(NewHero);
        }

        if (GameManager.instance.autoBattle == true)
        {
            abTurnsR = GameManager.instance.remainingAutobattleTurns;
            if(abTurnsR > 0)
            {
                autoBattle = true;
                Debug.Log("Autobattle is on, remaining autobattle turns: " + abTurns);
            }
        }

    }

    void Start()
    {
        battleStates = PerformAction.WAIT;
        currentCount = startingCount;
        //EnemysInBattle.AddRange (GameObject.FindGameObjectsWithTag ("Enemy"));
        //HerosInBattle.AddRange (GameObject.FindGameObjectsWithTag ("Hero"));


        HeroInput = HeroGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanel.SetActive(false);

        EnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        turnText.text = "Current Turn: " + CurrentTurn;

        switch (battleStates)
        {
            case (PerformAction.WAIT):

                //countdown thingies
                if (currentCount > 0)
                {
                    currentCount -= 1 * Time.deltaTime;
                    countdownText.text = currentCount.ToString("0");
                }
                else {
                    countdownText.text = "Action Time!";
                }

                //Battle things
                if (ChoicesMade >= HerosInBattle.Count)
                {
                    if(PerformList.Count == 0)
                    {
                        ChoicesMade = 0;
                        CurrentTurn++;
                        if (autoBattle == true)
                        {
                            abTurnsR--;
                            if(abTurnsR <= 0)
                            {
                                autoBattle = false;
                            }
                        }
                        currentCount = startingCount;
                        countdownText.text = "Action Time!";
                        WaitABit(2);
                    }
                    if (PerformList.Count >= 1)
                    {
                        countdownText.text = "";
                        battleStates = PerformAction.TAKEACTION;
                    }
                }
                break;
            case (PerformAction.TAKEACTION):
                PerformList = PerformList.OrderBy(x => x.attackersSpeed).ToList();
                PerformList.Reverse();
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    for (int i = 0; i < HerosInBattle.Count; i++)
                    {
                        if (PerformList[0].AttackersTarget == HerosInBattle[i])
                        {
                            ESM.HeroToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttackersTarget = HerosInBattle[UnityEngine.Random.Range(0, HerosInBattle.Count)];
                            ESM.HeroToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                        }
                    }

                }

                if (PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
                }

                battleStates = PerformAction.PERFORMACTION;
                break;

            case (PerformAction.PERFORMACTION):
                //idle
                break;

            case (PerformAction.CHECKALIVE):
                if (HerosInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                    //lose battle
                }
                else if (EnemysInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                    //win battle
                }
                else
                {
                    //call function
                    clearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                }
                break;

            case (PerformAction.LOSE):
                {
                    Debug.Log("You lost the battle");
                    for (int i = 0; i < HerosInBattle.Count; i++)
                    {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                    }

                    GameManager.instance.LoadSceneAfterBattle();
                    GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
                    GameManager.instance.enemysToBattle.Clear();
                }
                break;

            case (PerformAction.WIN):
                {
                    for(int i = 0; i<HerosInBattle.Count; i++)
                    {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                        HerosInBattle[i].GetComponent<HeroStateMachine>().hero.level.AddExp(1000);
                        Debug.Log("Gained 1000exp");
                    }

                    GameManager.instance.LoadSceneAfterBattle();
                    GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
                    GameManager.instance.enemysToBattle.Clear();
                }
                break;
        }

        if (autoBattle == false)
        {
            switch (HeroInput)
            {
                case (HeroGUI.ACTIVATE):
                    if (HeroesToManage.Count >= 1 && ChoicesMade < HerosInBattle.Count)
                    {
                        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                        HeroChoise = new HandleTurn();
                        //populate action buttons
                        if (autoBattle == false)
                        {
                            AttackPanel.SetActive(true);
                            CreateAttackButtons();
                        }
                        HeroInput = HeroGUI.WAITING;
                    }
                    break;
                case (HeroGUI.WAITING):
                    if (autoBattle == true)
                    {
                        AutoSelect();
                    }
                    break;
                case (HeroGUI.DONE):
                    HeroInputDone();
                    break;
            }
        }
    }

    public void CollectActions(HandleTurn input)
            {
                 PerformList.Add (input);
            }

    public void EnemyButtons()
    {
        //cleanup
        foreach(GameObject enemyBtn in enemyBtns)
        {
            enemyBtn.GetComponent<EnemySelectButton>().EnemyPrefab.transform.Find("Selector").gameObject.SetActive(false);
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();
        //create buttons for each enemy
        foreach(GameObject enemy in EnemysInBattle)
        {
            GameObject newButton = Instantiate (enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.theName;
            
            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer, false);
            enemyBtns.Add(newButton);
        }
    }

    //public void AutoSelect1()
    //{
    //    HeroChoise = new HandleTurn();
    //    HeroChoise.Attacker = HeroesToManage[i].name; //might be changed
    //    HeroChoise.attackersSpeed = HeroesToManage[i].GetComponent<HeroStateMachine>().hero.curSpeed;
    //    HeroChoise.AttackersGameObject = HeroesToManage[i];
    //    HeroChoise.Type = "Hero";
    //    int num = Random.Range(0, HeroesToManage[i].GetComponent<HeroStateMachine>().hero.attacks.Count);
    //    HeroChoise.choosenAttack = HeroesToManage[i].GetComponent<HeroStateMachine>().hero.attacks[num];
    //    HeroChoise.AttackersTarget = EnemysInBattle[Random.Range(0, EnemysInBattle.Count)];
    //    PerformList.Add(HeroChoise);
    //    ChoicesMade++;
    //    HeroesToManage.RemoveAt(0);

    //}

    public void AutoSelect()
    {
            //HeroChoise = new HandleTurn();
            HeroChoise.Attacker = HeroesToManage[0].name; //might be changed
            HeroChoise.attackersSpeed = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.curSpeed;
            HeroChoise.AttackersGameObject = HeroesToManage[0];
            HeroChoise.Type = "Hero";
            int num = UnityEngine.Random.Range(0, HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks.Count);
            HeroChoise.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[num];
            HeroChoise.AttackersTarget = EnemysInBattle[UnityEngine.Random.Range(0, EnemysInBattle.Count)];
            HeroInput = HeroGUI.DONE;
    }

    public void Input1() //attack button
    {
        HeroChoise.Attacker = HeroesToManage[0].name; //might be changed
        HeroChoise.attackersSpeed = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.curSpeed;
        HeroChoise.AttackersGameObject = HeroesToManage[0];
        HeroChoise.Type = "Hero";
        HeroChoise.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input2 (GameObject choosenEnemy) //select enemy / target
    {
        HeroChoise.AttackersTarget = choosenEnemy;
        HeroInput = HeroGUI.DONE;
    }

    void HeroInputDone()
    {
        PerformList.Add(HeroChoise);
        ChoicesMade++;
        //cleanup attack panel
        clearAttackPanel();

        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    void clearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    //create actiobuttons
    void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(AttackButton);

        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;
        Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(MagicAttackButton);


        //add flee button allowing to flee the battle
        //TODO: Only flee if no enemy is in ACTION state.
        //This will be only doable after implementing real turn system
        GameObject FleeButton = Instantiate(actionButton) as GameObject;
        Text FleeButtonText = FleeButton.transform.Find("Text").gameObject.GetComponent<Text>();
        FleeButtonText.text = "Flee";
        FleeButton.GetComponent<Button>().onClick.AddListener(() => Input5());
        FleeButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(FleeButton);

        //add flee button allowing to flee the battle
        //TODO: Only flee if no enemy is in ACTION state.
        //This will be only doable after implementing real turn system
        GameObject AutoSelectButton = Instantiate(actionButton) as GameObject;
        Text AutoSelectButtonText = AutoSelectButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AutoSelectButtonText.text = "Auto";
        AutoSelectButton.GetComponent<Button>().onClick.AddListener(() => AutoSelect());
        AutoSelectButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(AutoSelectButton);

        if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
        {
            foreach(BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent <Text>();
                MagicButtonText.text = magicAtk.attackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                atkBtns.Add(MagicButton);
            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }

    public void Input4(BaseAttack choosenMagic) //chosen magic attack
    {
        HeroChoise.Attacker = HeroesToManage[0].name; //might be changed
        HeroChoise.AttackersGameObject = HeroesToManage[0];
        HeroChoise.Type = "Hero";

        HeroChoise.choosenAttack = choosenMagic;
        MagicPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input3() //switching to magic attacks
    {
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(true);
    }
    public void Input5() //fleeing from battle and clearing the current battle information
    {
        for (int i = 0; i < HerosInBattle.Count; i++)
        {
            HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
        }

        Debug.Log("Fleed from battle");
        GameManager.instance.LoadSceneAfterBattle();
        GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
        GameManager.instance.enemysToBattle.Clear();
    }

    public void ToggleAutoBattle()
    {
        autoBattle = !autoBattle;
        if(autoBattle == true)
        {
            autoText.text = "Auto: ON" + Environment.NewLine + "Turns: " + abTurnsR;
        }
        else
        {
            autoText.text = "Auto: OFF";
        }
    }

    IEnumerator WaitABit(int sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
