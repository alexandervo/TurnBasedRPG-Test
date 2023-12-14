using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    
    //IeNumerator
    public List<GameObject> EnemyToAttack = new List<GameObject>();
    public GameObject NewEnemyToAttack;

    public List<StatusList> StatusList = new List<StatusList>();
    
    private bool actionStarted = false;
    public bool counterAttack = false;

    private Vector3 startPosition;
    private float animSpeed = 15f;
    //dead
    private bool alive = true;
    //hero panel
    //private HeroPanelStats stats;
    public GameObject HeroPanel;
    //private Transform HeroPanelSpacer;
    public GameObject Selector;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public RageBar rageBar;
    //scream things
    public GameObject scream;
    public TextMeshProUGUI screamText;
    private int critHits;

    //private int rageAmount;
    //testing
    public bool dodgedAtt = false;
    

    private bool attackNext = false;
    //needed for melee / magic animations / hero movements
    public bool isMelee;
    private float hitChance;

    public GameObject FloatingText;
    public GameObject HealVFX;

    private Animator heroAnim;
    private AudioSource heroAudio;

    private bool isCriticalH = false;

    private void Awake()
    {
        //SetParams();
        //hero.level = new Level(10, OnLevelUp);
        //UpdateHeroPanel();
       
    }

    void Start()
    {
        TMP_Text heroName = hero.displayNameText;
        heroName.text = hero.theName.ToString();
        //Set player rage
        rageBar.SetRageBarSize(((hero.curRage * 100) / hero.maxRage) / 100);

        heroAnim = GetComponent<Animator>();
        heroAudio = GetComponent<AudioSource>();
        //find spacer object
        //HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
        
        //create panel and fill in info
        //CreateHeroPanel();
        startPosition = transform.position;

        // cur_cooldown = Random.Range (0, 2.5f);
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.PROCESSING;

        //testing chain attacking
        //hero.curATK = 1000;
        //hero.minATK = hero.curATK;
        //hero.maxATK = 1500;

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
                    BSM.HeroesInBattle.Remove(gameObject);
                    //not able to manage the hero anymore
                    BSM.HeroesToManage.Remove(gameObject);
                    //deactivate the selector
                    Selector.SetActive(false);
                    //reset gui
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove hero from performlist
                    if (BSM.HeroesInBattle.Count > 0)
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
                                    BSM.PerformList[i].AttackersTarget.Add(BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)]);
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

        scream.SetActive(true);
        screamText.text = BSM.PerformList[0].choosenAttack.name.ToString() + "!";
        yield return new WaitForSeconds(0.25f);

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
        scream.SetActive(false);

        heroAnim.Play("Attack");
        heroAudio.Play();
        
        //do damage
        if (BSM.PerformList[0].choosenAttack.isAttack == false)
        {
            ApplyBuffsDebuffs();
        }
        else
        {
            yield return new WaitForSeconds(0.7f);
            DoDamage();
            yield return new WaitForSeconds(0.25f);

            //check for counterattack
            if (BSM.PerformList[0].AttackersTarget[0].GetComponent<EnemyStateMachine>().counterAttack == true)
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
        //if (BSM.PerformList[0].AttackersTarget[0].CompareTag("Enemy"))
        //{
            if (isMelee == true && BSM.EnemiesInBattle.Count > 0 && BSM.PerformList[0].AttackersTarget[0].GetComponent<EnemyStateMachine>().enemy.curHP <= 0 )
            {
                StartCoroutine(AttackNextTarget());
                while (attackNext == true)
                {
                    yield return null;
                }
            }
        //}
        
        //
        if (isMelee == false)
        {
            yield return new WaitForSeconds(1f);
        }
        //animate back to start position
        if (isMelee && hero.curHP > 0)
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
            BSM.battleStates = BattleStateMachine.PerformAction.START;
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



    public void TakeDamage(float getDamageAmount, bool isCriticalE, float enemyHit, bool isDodgeable, bool isCounterAttack)
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
            DamagePopup(isCriticalE, getDamageAmount, false);
            //health bar
            healthBar.SetSize(((hero.curHP * 100) / hero.baseHP) / 100);
   
        }
        else
        {
            dodgedAtt = true;
            DodgePopup();
            AddRage(10);
        }

        if (!isCounterAttack && hero.curHP > 0 && isDodgeable == true && Random.Range(0, 100) <= 100)
        {
            if (BSM.PerformList[0].AttackersGameObject.GetComponent<EnemyStateMachine>().secondAttackRunning == false)
            {
                StartCoroutine(CounterAttack());
            }
        }

        AddRage(10);
        //UpdateHeroPanel();
        //rage bar
        UpdateRageBar();
    }

    //do damage
    public void DoDamage()
    {

        float minMaxAtk = Mathf.Round(Random.Range(hero.minATK, hero.maxATK));
        float calc_damage = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;

        //testing multiple targets
        if (BSM.PerformList[0].AttackersTarget.Count > 1)
        {
            if (BSM.PerformList[0].AttackersTarget.Count > BSM.EnemiesInBattle.Count)
            {
                
                for (int i = 0; i < BSM.EnemiesInBattle.Count; i++)
                {
                    CalcDamageForEachTarget(calc_damage, i);
                }

            }
            else
            {
                for (int i = 0; i < BSM.PerformList[0].AttackersTarget.Count; i++)
                {
                    CalcDamageForEachTarget(calc_damage, i);
                }
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
                isCriticalH = true;
                calc_damage = Mathf.Round(calc_damage * hero.critDamage);
                AddRage(10);
            }
            BSM.PerformList[0].AttackersTarget[0].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, hero.curHit, isMelee, false);
        }
        
        //mana bar things
        hero.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
        if (hero.curMP <= 0)
        {
            hero.curMP = 0;
        }
        manaBar.SetSize(((hero.curMP * 100) / hero.baseMP) / 100);

        AddRage(10);
        isCriticalH = false;
        //rage bar
        UpdateRageBar();
    }

    private IEnumerator CounterAttack()
    {
        if (counterAttack)
        {
            yield break;
        }

        counterAttack = true;

        yield return new WaitForSeconds(0.5f);
        heroAnim.Play("Attack");
        heroAudio.Play();
        yield return new WaitForSeconds(0.25f);
        float minMaxAtk = Mathf.Round(Random.Range(hero.minATK, hero.maxATK));

        if (Random.Range(0, 100) <= hero.curCRIT)
        {
            isCriticalH = true;
            minMaxAtk = Mathf.Round(minMaxAtk * hero.critDamage);
            AddRage(10);
        }
        BSM.PerformList[0].AttackersGameObject.GetComponent<EnemyStateMachine>().TakeDamage(minMaxAtk, isCriticalH, hero.curHit, true, counterAttack);
        AddRage(10);
        yield return new WaitForSeconds(0.5f);
        counterAttack = false;
    }

    public void ApplyBuffsDebuffs()
    {
        float minMaxAtk = Mathf.Round(Random.Range(hero.minATK, hero.maxATK));
        float calc_buff = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;
        //actualy code
        bool isHeal = true;

        int count = BSM.PerformList[0].choosenAttack.attackTargets;
        if(BSM.HeroesInBattle.Count < count)
        {
            count = BSM.HeroesInBattle.Count;
        }

        for (int i = 0; i < count; i++)
        {
            BSM.PerformList[0].AttackersTarget[i].GetComponent<HeroStateMachine>().AcceptBuffsDebuffs(calc_buff, isHeal);
        }
        //mana bar things
        hero.curMP -= BSM.PerformList[0].choosenAttack.attackCost;
        if (hero.curMP <= 0)
        {
            hero.curMP = 0;
        }
        manaBar.SetSize(((hero.curMP * 100) / hero.baseMP) / 100);

        AddRage(10);
        //rage bar
        UpdateRageBar();
    }

    public void AcceptBuffsDebuffs(float buff_value, bool isHeal)
    {
        if (isHeal)
        {
            RestoreHP(buff_value, 100);
        }
    }

    ////create hero panel
    //void CreateHeroPanel()
    //{
    //    HeroPanel = Instantiate(HeroPanel) as GameObject;
    //    stats = HeroPanel.GetComponent<HeroPanelStats>();
    //    stats.HeroName.text = hero.theName;
    //    stats.HeroHP.text = "HP: " + hero.curHP + "/" + hero.baseHP;
    //    stats.HeroMP.text = "MP: " + hero.curMP + "/" + hero.baseMP;

    //    //ProgressBar = stats.ProgressBar;
    //    HeroPanel.transform.SetParent(HeroPanelSpacer, false);
    //}

    ////update visual stats upon taking damage / heal
    //void UpdateHeroPanel()
    //{
    //    stats.HeroHP.text = "HP: " + hero.curHP + "/" + hero.baseHP;
    //    stats.HeroMP.text = "MP: " + hero.curMP + "/" + hero.baseMP;
    //}

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
    
    private void DamagePopup(bool isCritical, float DamageAmount, bool isHeal)
    {
        var go = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (isCritical == true)
        {
            go.GetComponentInChildren<SpriteRenderer>().enabled = true;
            go.GetComponent<TextMeshPro>().fontSize = 5;
            go.GetComponent<TextMeshPro>().color = Color.red;
        }

        else if (isHeal)
        {
            go.GetComponentInChildren<SpriteRenderer>().enabled = false;
            go.GetComponent<TextMeshPro>().color = Color.green;
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

    public void RestoreHP(float damage, float percentage)
    {
        float vampAmount = Mathf.Round((damage * percentage) / 100);
        hero.curHP += vampAmount;
        if (hero.curHP > hero.baseHP)
        {
            hero.curHP = hero.baseHP;
        }
        DamagePopup(false, vampAmount, true);
        Instantiate(HealVFX, transform.position, Quaternion.identity, transform);
        healthBar.SetSize(((hero.curHP * 100) / hero.baseHP) / 100);
    }

    void CalcDamageForEachTarget(float calc_damage, int i)
    {
        if (Random.Range(0, 100) <= hero.curCRIT)
        {
            isCriticalH = true;
            calc_damage = Mathf.Round(calc_damage * hero.critDamage);
            critHits++;
        }
        float opponentDef = BSM.PerformList[0].AttackersTarget[i].GetComponent<EnemyStateMachine>().enemy.curDEF;
        calc_damage -= opponentDef;

        if (calc_damage < 0)
        {
            calc_damage = 0;
        }
        BSM.PerformList[0].AttackersTarget[i].GetComponent<EnemyStateMachine>().TakeDamage(calc_damage, isCriticalH, hero.curHit, isMelee, false);
    }


    //If we killed a target, chase next one. 
    private IEnumerator AttackNextTarget()
    {
        if (attackNext)
        {
            yield break;
        }

        attackNext = true;
        GameObject NewEnemyToAttack = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
        BSM.PerformList[0].AttackersTarget[0] = NewEnemyToAttack;
        Vector3 newEnemyPosition = new Vector3(NewEnemyToAttack.transform.position.x + 0.6f, NewEnemyToAttack.transform.position.y - 0.2f /*, HeroToAttack.transform.position.z */);
        while (MoveTowardsEnemy(newEnemyPosition))
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);

        heroAnim.Play("Attack");
        heroAudio.Play();
        yield return new WaitForSeconds(0.7f);
        DoDamage();
        yield return new WaitForSeconds(0.25f);
        //check for counterattack
        if (BSM.PerformList[0].AttackersTarget[0].GetComponent<EnemyStateMachine>().counterAttack == true)
        {
            yield return new WaitForSeconds(1.0f);
        }
        attackNext = false;
    }

    //TODO LIST OF SOME SORT
    // 1) Hide mechanic
    //    hidden heroes can't be hit or targeted unless enemy has vision passive skill
    // 2) Exorcism / holy mechanic to counter undead
    // 3) Heal over time (buff that heals ally(allies) at the end or in the beginning of the turn)
    // 4) Mana restoration skill
    // 5) Poison over time. Same as heal over time, but drains %% HP / %% MP at the end of the turn.
    // 6) Ressurect skill. Similar to what RiseUndead method in Enemy state machine does. Cleric / healer skill.
    //

}
