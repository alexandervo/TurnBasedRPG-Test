using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BattleStateMachine;

public class EnemyStateMachine : MonoBehaviour
{
    
    public BaseEnemy enemy;
    private BattleStateMachine BSM;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    /* for pregressbar
    //
    // private float cur_cooldown = 0f;
    // private float max_cooldown = 5f;
    */
    private Vector3 startposition;
    //timeforaction
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 10f;
    public GameObject Selector;
    //enemy panel
    private EnemyPanelStats stats;
    public GameObject EnemyPanel;
    private Transform EnemyPanelSpacer;
    public HealthBar healthBar;
    public GameObject FloatingText;

    private bool isCriticalE = false;

    //alive
    private bool alive = true;

    void Start()
    {
        // According to tutorial: 
        // currentState = TurnState.PROCESSING;
        // we skip it cause of stupidass progress bar
        //find spacer object
        EnemyPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("EnemyPanel").transform.Find("EnemyPanelSpacer");

        //create panel and fill in info
        CreateEnemyPanel();
        currentState = TurnState.PROCESSING;
        Selector.SetActive (false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ();
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                // UpgradeProgressBar ();
                currentState = TurnState.CHOOSEACTION;
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction ();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction ());
                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag of enemy
                    this.gameObject.tag = "DeadEnemy";
                    //not attackable by heroes
                    BSM.EnemysInBattle.Remove(this.gameObject);
                    //disable the selector
                    Selector.SetActive (false);
                    //remove all inputs
                    if (BSM.EnemysInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                            {
                                BSM.PerformList.Remove(BSM.PerformList[i]);
                            }
                            else if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                            {
                                BSM.PerformList[i].AttackersTarget = BSM.EnemysInBattle[Random.Range(0, BSM.EnemysInBattle.Count)];
                            }
                        }
                    }
                    //change the color to gray / play death animation
                    this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
                    //make not alive
                    alive = false;
                    //reset enemy buttons
                    BSM.EnemyButtons();
                    //check alive
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
                }
                break;
        }
    }

    /* Progress bar shit
    void UpgradeProcessBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        
        if(cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.CHOOSEACTION;
            }
    }
    */

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn ();
        myAttack.Attacker = enemy.theName;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        //Target choice: Randomly choose the target from list. Editable for later.
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
        
        int num = Random.Range(0, enemy.attacks.Count);
        myAttack.choosenAttack = enemy.attacks[num];
        //Debug.Log(this.gameObject.name + " has choosen " + myAttack.choosenAttack.attackName + " and does " + myAttack.choosenAttack.attackDamage + " damage.");

        BSM.CollectActions (myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x-0.3f, HeroToAttack.transform.position.y+0.3f /*, HeroToAttack.transform.position.z */);
        while (MoveTowardsEnemy(heroPosition))
        {
            yield return null;
        }
        //wait a bit till animation of attack plays. Might wanna change later on based on animation.
        yield return new WaitForSeconds(.5f);
        //do damage
        DoDamage ();

        //animate back to start position
        Vector3 firstPosition = startposition;
        while (MoveTowardsStart(firstPosition))
        {
            yield return null;
        }
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        //end coroutine
        actionStarted = false;
        //reset this enemy state
        //cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }

    //Move sprite towards target
    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    //return the sprite towards starting position on battlefield
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDamage()
    {
        //play attack animation

        //do damage
        float calc_damage = enemy.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        //critical strikes
        if (Random.Range(0f, 1f) <= enemy.curCRIT)
        {
            Debug.Log("Critical hit!");
            isCriticalE = true;
            calc_damage *= enemy.critDamage;
        }
        //add damage formula later on
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage, isCriticalE);
        isCriticalE = false;
    }

    public void TakeDamage(float getDamageAmount, bool isCriticalH)
    {       
        //play hurt animation

        //take damage
        enemy.curHP -= getDamageAmount;
        if (enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.DEAD;
        }
        //show popup damage
        var go = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (isCriticalH == true)
        {
            go.GetComponent<TextMeshPro>().color = Color.red;

        }
        else
        {
            go.GetComponent<TextMeshPro>().color = Color.yellow;
        }
        go.GetComponent<TextMeshPro>().text = getDamageAmount.ToString();
        //update health bar
        healthBar.SetSize(((enemy.curHP * 100) / enemy.baseHP) / 100);
        UpdateEnemyPanel();
    }

    void CreateEnemyPanel()
    {
        EnemyPanel = Instantiate(EnemyPanel) as GameObject;
        stats = EnemyPanel.GetComponent<EnemyPanelStats>();
        stats.EnemyName.text = enemy.theName;
        stats.EnemyHP.text = "HP: " + enemy.curHP + "/" + enemy.baseHP;
        stats.EnemyMP.text = "MP: " + enemy.curMP + "/" + enemy.baseMP;

        //ProgressBar = stats.ProgressBar;
        EnemyPanel.transform.SetParent(EnemyPanelSpacer, false);
    }

    //update visual stats upon taking damage / heal
    void UpdateEnemyPanel()
    {
        stats.EnemyHP.text = "HP: " + enemy.curHP + "/" + enemy.baseHP;
        stats.EnemyMP.text = "MP: " + enemy.curMP + "/" + enemy.baseMP;
    }


}

