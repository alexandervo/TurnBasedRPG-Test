using System.Collections;
//using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HeroStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    public BaseHero hero;
    
    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    /* for pregressbar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private Image ProgressBar;
    */
    public GameObject Selector;
    //IeNumerator
    public List<GameObject> EnemyToAttack = new List<GameObject>();
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;
    //dead
    private bool alive = true;
    //hero panel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;
    public HealthBar healthBar;
    public RageBar rageBar;
    private int critHits;

    private int rageAmount;
    //testing
    public bool dodgedAtt = false;

    //needed for melee / magic animations / hero movements
    public bool isMelee;
    private float hitChance;

    public GameObject FloatingText;

    private Animator heroAnim;
    private AudioSource heroAudio;

    private bool isCriticalH = false;

    private void Awake()
    {
        //SetParams();
        hero.level = new Level(1, OnLevelUp);
        //UpdateHeroPanel();
       
    }

    void Start()
    {
        critHits = 0;
        //Set player rage
        rageBar.SetRageBarSize((((hero.curRage * 100) / hero.maxRage) / 100));

        heroAnim = GetComponent<Animator>();
        heroAudio = GetComponent<AudioSource>();
        //find spacer object
        HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
        
        //create panel and fill in info
        //CreateHeroPanel();
        // dadsa
        startPosition = transform.position;

        // cur_cooldown = Random.Range (0, 2.5f);
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.PROCESSING;

    }

    void Update()
    {
        //exp add by hand
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hero.level.AddExp(100);
        }
        switch(currentState) 
        {
            case (TurnState.PROCESSING):
            // UpgradeProgressBar ();
            currentState = TurnState.ADDTOLIST;
            break;

            case (TurnState.ADDTOLIST):
                BSM.HeroesToManage.Add(gameObject);
                currentState = TurnState.WAITING;
            break;

            case (TurnState.WAITING):
                //idle

            break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
            break;

            case (TurnState.DEAD):
                if(!alive)
                {
                    return;
                }
                else
                {
                    //change tag of hero
                    gameObject.tag = "DeadHero";
                    //not attackable by enemy
                    BSM.HerosInBattle.Remove(gameObject);
                    //not able to manage the hero anymore
                    BSM.HeroesToManage.Remove(gameObject);
                    //deactivate the selector
                    Selector.SetActive(false);
                    //reset gui
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove hero from performlist
                    if (BSM.HerosInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (BSM.PerformList[i].AttackersGameObject == gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }
                                else if (BSM.PerformList[i].AttackersTarget[0] == gameObject)
                                {
                                    BSM.PerformList[i].AttackersTarget.Remove(gameObject);
                                    BSM.PerformList[i].AttackersTarget.Add(BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)]);
                                }
                            }
                        }
                    }
                    //change appearance / play death animation
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
                    //make not alive
                    alive = false;
                    //reset hero input
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE; 
                }
            break;
        }
    }
    /* Progress bar shit
    void UpgradeProcessBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
              if(cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.ADDTOLIST;
            }
    }
    */
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;
        //animate the enemy near the hero to attack
        if (BSM.PerformList[0].choosenAttack.attackType != "Melee")
        {
            isMelee = false;
        }
        else
        {
            isMelee = true;
        }

        if (isMelee == true)
        {
            Vector3 enemyPosition = new Vector3(EnemyToAttack[0].transform.position.x + 0.6f, EnemyToAttack[0].transform.position.y - 0.2f /*, HeroToAttack.transform.position.z */);
            while (MoveTowardsEnemy(enemyPosition))
            {
                yield return null;
            }
        }

        //wait a bit till animation of attack plays. Might wanna change later on based on animation.
        yield return new WaitForSeconds(0.25f);
        heroAnim.Play("Attack");
        heroAudio.Play();
        yield return new WaitForSeconds(0.7f);
        //do damage
        DoDamage();
        //
        if(isMelee == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //animate back to start position
        if (isMelee)
        {
            Vector3 firstPosition = startPosition;
            while (MoveTowardsStart(firstPosition))
            {
                yield return null;
            }
        }
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            //reset this enemy state
            //cur_cooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }
        //end coroutine
        actionStarted = false;
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


    public void TakeDamage(float getDamageAmount, bool isCriticalE, float enemyHit, bool isDodgeable)
    {
        hitChance = (enemyHit / hero.curDodge) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
        if (isDodgeable == false)
        {
            hitChance = 100;
        }
        if (Random.Range(0, 101) <= hitChance) //in 20 outs out of 100 we dodge
        {
            dodgedAtt = false;
            heroAnim.Play("Hurt");

            hero.curHP -= getDamageAmount;
            if (hero.curHP <= 0)
            {
                hero.curHP = 0;
                currentState = TurnState.DEAD;
                heroAnim.Play("Die");
            }

            //show popup damage
            DamagePopup(isCriticalE, getDamageAmount);
            //health bar
            healthBar.SetSize(((hero.curHP * 100) / hero.baseHP) / 100);
   
        }
        else
        {
            dodgedAtt = true;
            DodgePopup();
            AddRage(10);
        }
        AddRage(20);
        //UpdateHeroPanel();
        //rage bar
        UpdateRageBar();
    }

    //do damage
    void DoDamage()
    {
        float minMaxAtk = Mathf.Round(Random.Range(hero.minATK, hero.maxATK));
        //float calc_damage = Mathf.Round(hero.curATK + BSM.PerformList[0].choosenAttack.attackDamage);
        float calc_damage = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;
        //play attack sprites
        //critical strikes

        //add damage formula later on



        //testing multiple targets
        if (BSM.PerformList[0].choosenAttack.attackTargets > 1)
        {
            for (int i = 0; i < BSM.PerformList[0].choosenAttack.attackTargets; i++)
            {
                if (Random.Range(0, 100) <= hero.curCRIT)
                {
                    Debug.Log("Critical hit!");
                    isCriticalH = true;
                    calc_damage = Mathf.Round(calc_damage * hero.critDamage);
                    critHits++;
                }
                float opponentDef = EnemyToAttack[i].GetComponent<EnemyStateMachine>().enemy.curDEF;
                calc_damage -= opponentDef;
                EnemyToAttack[i].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, hero.curHit, isMelee);
            }
            if (critHits >= 1)
            {
                AddRage(10);
                critHits = 0;
            }

        }
        else
        {
            if (Random.Range(0, 100) <= hero.curCRIT)
            {
                Debug.Log("Critical hit!");
                isCriticalH = true;
                calc_damage = Mathf.Round(calc_damage * hero.critDamage);
                AddRage(10);
            }
            EnemyToAttack[0].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, hero.curHit, isMelee);
        }
        
        AddRage(10);
        isCriticalH = false;
        //rage bar
        UpdateRageBar();
    }

    //create hero panel
    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<HeroPanelStats>();
        stats.HeroName.text = hero.theName;
        stats.HeroHP.text = "HP: " + hero.curHP + "/" + hero.baseHP;
        stats.HeroMP.text = "MP: " + hero.curMP + "/" + hero.baseMP;

        //ProgressBar = stats.ProgressBar;
        HeroPanel.transform.SetParent(HeroPanelSpacer, false);
    }

    //update visual stats upon taking damage / heal
    void UpdateHeroPanel()
    {
        stats.HeroHP.text = "HP: " + hero.curHP + "/" + hero.baseHP;
        stats.HeroMP.text = "MP: " + hero.curMP + "/" + hero.baseMP;
    }

    void UpdateRageBar()
    {
        rageBar.SetRageBarSize(((hero.curRage * 100) / hero.maxRage) / 100);
    }

    //Add rage based on the events like attack, critical attack, damage taken, dodge, etc.
    void AddRage(int rageAmount)
    {
        if (hero != null)
        {
            hero.curRage += rageAmount;
            if (hero.curRage >= hero.maxRage)
            {
                hero.curRage = hero.maxRage;
            }
        }
    }

    void DodgePopup()
    {
        heroAnim.Play("Hurt"); // replace with step away animation later
        var go = Instantiate(FloatingText, transform.position, Quaternion.identity, transform); //tell that we dodged and no damage is dealt
        go.GetComponentInChildren<SpriteRenderer>().enabled = false;
        go.GetComponent<TextMeshPro>().fontSize = 2;
        go.GetComponent<TextMeshPro>().color = Color.white;
        go.GetComponent<TextMeshPro>().text = "DODGE";
    }
    
    private void DamagePopup(bool isCritical, float DamageAmount)
    {
        var go = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (isCritical == true)
        {
            go.GetComponentInChildren<SpriteRenderer>().enabled = true;
            go.GetComponent<TextMeshPro>().fontSize = 6;
            go.GetComponent<TextMeshPro>().color = Color.red;
        }
        else
        {
            go.GetComponentInChildren<SpriteRenderer>().enabled = false;
            go.GetComponent<TextMeshPro>().color = new Color32(197, 164, 0, 255);
        }
        go.GetComponent<TextMeshPro>().text = DamageAmount.ToString();
    }
    public void OnLevelUp()
    {
        print("Hero leveled up!");
        hero.unspentStatPoints += hero.statpointsPerLevel;
        hero.strength += hero.statIncreasePerLevel;
        hero.intellect += hero.statIncreasePerLevel;
        hero.dexterity += hero.statIncreasePerLevel;
        hero.agility += hero.statIncreasePerLevel;
        hero.stamina += hero.statIncreasePerLevel;

    }

}
