using JetBrains.Annotations;
using System.Collections;
//using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject EnemyToAttack;
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

    private int rageAmount;
    //testing
    public bool dodgedAtt = false;

    //needed for melee / magic animations / hero movements
    private bool isMelee;
    private float hitChance;

    public GameObject FloatingText;

    private Animator heroAnim;
    private AudioSource heroAudio;

    private bool isCriticalH = false;

    private void Awake()
    {
        SetParams();
        hero.level = new Level(1, OnLevelUp);
        //UpdateHeroPanel();
       
    }

    void Start()
    {
        
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
                BSM.HeroesToManage.Add(this.gameObject);
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
                    this.gameObject.tag = "DeadHero";
                    //not attackable by enemy
                    BSM.HerosInBattle.Remove(this.gameObject);
                    //not able to manage the hero anymore
                    BSM.HeroesToManage.Remove(this.gameObject);
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
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }

                                if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
                                }
                            }
                        }
                    }
                    //change appearance / play death animation
                    this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(61, 61, 61, 255);
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

        if (isMelee)
        {
            Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 0.6f, EnemyToAttack.transform.position.y - 0.2f /*, HeroToAttack.transform.position.z */);
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

        //try mass magic
        //if (!isMelee && BSM.PerformList[0].choosenAttack.attackTargets == "Multiple")
        //{
        //    
        //    yield return new WaitForSeconds(0.25f);
        //    heroAnim.Play("Attack");
        //    heroAudio.Play();
        //    yield return new WaitForSeconds(0.7f);
        //    //do damage
        //    DoDamage();
       // }

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
        if (!isDodgeable)
        {
            hitChance = 101;
        }
        if (Random.Range(1, 101) <= hitChance) //in 20 outs out of 100 we dodge
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
        if (Random.Range(0f, 1f) <= hero.curCRIT)
        {
            Debug.Log("Critical hit!");
            isCriticalH = true;
            calc_damage = Mathf.Round(calc_damage * hero.critDamage);
            AddRage(30);
        }
        //do damage
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, hero.curHit, isMelee);
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

    private void SetParams()
    {
        //Calculate HP based on Stats
        hero.baseHP = Mathf.Round(hero.strength * hero.hpPerStr) + (hero.stamina * hero.hpPerSta);
        hero.curHP = hero.baseHP;

        //Calculate MP based on stats
        hero.baseMP = Mathf.Round(hero.intellect * hero.mpPerInt);
        hero.curMP = hero.baseMP;

        //Calculate Attack based on stats
        hero.baseATK = Mathf.Round((hero.strength * hero.atkPerStr) + (hero.intellect * hero.atkPerInt) / 10);
        hero.curATK = hero.baseATK;

        hero.maxATK = hero.baseATK + Random.Range(150, 500);
        hero.minATK = hero.baseATK;

        //Calculate HIT based on stats
        hero.baseHit = Mathf.Round(hero.dexterity * hero.hitPerDex);
        hero.curHit = hero.baseHit;

        //Calculate dodge based on stats
        hero.baseDodge = Mathf.Round(hero.agility * hero.dodgePerAgi);
        hero.curDodge = hero.baseDodge;

        //calculate def based on stats
        hero.baseDEF = Mathf.Round((hero.stamina * hero.defPerSta) / 2);
        hero.curDEF = hero.baseDEF;

        //calculate critrate based on stats
        //hero.curCRIT = hero.baseCRIT;

        //calculate speed based on stats
        hero.baseSpeed = Mathf.Round(hero.agility * hero.spdPerAgi);
        hero.curSpeed = hero.baseSpeed;
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
    
    void DamagePopup(bool isCritical, float DamageAmount)
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
        SetParams();
        //UpdateHeroPanel();
    }

}
