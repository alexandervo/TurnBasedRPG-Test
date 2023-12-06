using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static BattleStateMachine;
using UnityEditor;
using static GameManager;

public class EnemyStateMachine : MonoBehaviour
{
    
    public BaseEnemy enemy;

    [System.Serializable]
    public class SkillSet
    {
        public BaseAttack possibleSkill;
        [Range(0,100)]public int skillSpawnChance = 25;
    }

    public class PassiveSkillSet
    {
        public PassiveSkill posPassive;
        [Range(0, 100)] public int skillSpawnChance = 25;
    }

    public List<SkillSet> PossibleSkills = new List<SkillSet>();
    public List<PassiveSkillSet> PossiblePassives = new List<PassiveSkillSet>();

    //public List<ScriptableObject> Skills = new List<ScriptableObject>();

    private BattleStateMachine BSM;
    //private BaseHero hero;

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
    public GameObject HealVFX;
    public GameObject RessurectVFX;

    private bool isMelee;
    private float hitChance;

    private Animator enemyAnim;
    private AudioSource enemyAudio;

    private bool isCriticalE = false;
    
    //For testing purpouses
    public bool doubleHit;
    private int killStreak = 0;

    //alive
    private bool alive = true;

    private void Awake()
    {
        SetParams();
        PopulateSkilllist();
        doubleHit = true;
    }

    void Start()
    {
        // According to tutorial: 
        // currentState = TurnState.PROCESSING;
        // we skip it cause of stupidass progress bar
        //find spacer object
        EnemyPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("EnemyPanel").transform.Find("EnemyPanelSpacer");

        //create panel and fill in info
        //CreateEnemyPanel();
        currentState = TurnState.CHOOSEACTION;
        Selector.SetActive (false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ();
        startposition = transform.position;

        enemyAnim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                if(BSM.battleStates == PerformAction.WAIT)
                {
                    currentState = TurnState.CHOOSEACTION;
                }
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
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
                            if (i != 0)
                            {
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }
                                if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.EnemysInBattle[Random.Range(0, BSM.EnemysInBattle.Count)];
                                }
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
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.theName;
        myAttack.attackersSpeed = enemy.curSpeed;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        //Target choice: Randomly choose the target from list. Editable for later.
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
        
        int num = Random.Range(0, enemy.attacks.Count);
        myAttack.choosenAttack = enemy.attacks[num];

        if (myAttack.choosenAttack.attackType != "Melee")
        {
            isMelee = false;
        }
        else
        {
            isMelee = true;
        }

        
        //Debug.Log(this.gameObject.name + " has choosen " + myAttack.choosenAttack.attackName + " and does " + myAttack.choosenAttack.attackDamage + " damage.");

        BSM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack

        if (isMelee)
        {
            Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x-0.6f, HeroToAttack.transform.position.y+0.3f /*, HeroToAttack.transform.position.z */);
            while (MoveTowardsEnemy(heroPosition))
            {
                yield return null;
            }
        }
        //wait a bit till animation of attack plays. Might wanna change later on based on animation.
        yield return new WaitForSeconds(0.25f);

        enemyAnim.Play("Attack");
        enemyAudio.Play();
        DoDamage();
        yield return new WaitForSeconds(0.75f);

        //Double Hit mechanic testing
        //If target died from first attack, do not attack for the second time
        //If we intend to attack, it has 35% chance to do so

        if (doubleHit && HeroToAttack.GetComponent<HeroStateMachine>().hero.curHP > 0)
        {
            if (Random.Range(0, 100) < 35)
            {
                enemyAnim.Play("Attack");
                enemyAudio.Play();
                DoDamage();
                yield return new WaitForSeconds(0.75f);
            }
        }

        //testing kill streak mechanics
        //after killing one target the killer should choose next one and attack it and do it untill he can't kill the next target
        if (HeroToAttack.GetComponent<HeroStateMachine>().hero.curHP <= 0)
        {
            killStreak++;
            Debug.Log("Kill Streak = " + killStreak);
        }
              
        if (isMelee)
        {
            //animate back to start position
            Vector3 firstPosition = startposition;
            while (MoveTowardsStart(firstPosition))
            {
                yield return null;
            }
        }
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset the battle state machine -> set to wait
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;

        //BSM.battleStates = BattleStateMachine.PerformAction.START;
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
        //dodge / hit calculations for all attack types (magic perfect hit to be added later)
        //get enemy hit value get target dodge value
        //hit / dodge x 100 = chance to hit call it hitChance
        //if hit > dodge, just proceed if hit < dodge random range (1, 100) < hitChance =  
        //
                //do damage
        float minMaxAtk = Mathf.Round(Random.Range(enemy.minATK, enemy.maxATK));
        //float calc_damage = enemy.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        float calc_damage = minMaxAtk + BSM.PerformList[0].choosenAttack.attackDamage;
        //critical strikes
        if (Random.Range(0, 100) <= enemy.curCRIT)
        {
            Debug.Log("Critical hit!");
            isCriticalE = true;
            calc_damage = Mathf.Round(calc_damage * enemy.critDamage);
        }

        //add damage formula later on
        float opponentDef = HeroToAttack.GetComponent<HeroStateMachine>().hero.curDEF;
        calc_damage -= opponentDef;
        if (calc_damage < 0)
        {
            calc_damage = 0;
        }

        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage, isCriticalE, enemy.curHit, isMelee);
        if (HeroToAttack.GetComponent<HeroStateMachine>().dodgedAtt == false)
        {
            VampireHP(calc_damage);
        }
    
        isCriticalE = false;
    }

    public void TakeDamage(float getDamageAmount, bool isCriticalH, float heroHit, bool isDodgeable)
    {
        //Calculate if the attack hits
        hitChance = (heroHit / enemy.curDodge) * 100; //(80 / 100) * 100 = 80%    (200 / 100) * 100 = 200
        if (!isDodgeable)
        {
            hitChance = 100;
        }
        if (Random.Range(1, 100) <= hitChance) //in 20 outs out of 100 we dodge
        {
            enemyAnim.Play("Hurt");
            //new WaitForSeconds(.25f);
            //take damage
            getDamageAmount -= enemy.curDEF;
            if (getDamageAmount < 0)
            {
                getDamageAmount = 0;
            }

            enemy.curHP -= getDamageAmount;
            if (enemy.curHP <= 0)
            {
                enemy.curHP = 0;
                //passive ressurect skill testing (Chance%, HP%)

                SelfRessurect(30, 50);

            }
            //show popup damage
            DamagePopup(isCriticalH, getDamageAmount, false);
            //update health bar
            healthBar.SetSize(((enemy.curHP * 100) / enemy.baseHP) / 100);
        }
        else
        {
            DodgePopup();
        }
        //UpdateEnemyPanel();
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

    private void SetParams()
    {
        //for randomness

        enemy.strength = Random.Range(15, 25);
        enemy.intellect = Random.Range(15, 25);
        enemy.dexterity = Random.Range(15, 25);
        enemy.agility = Random.Range(10, 25);
        enemy.stamina = Random.Range(15, 25);

        //Calculate HP based on Stats
        enemy.baseHP = Mathf.Round(enemy.strength * enemy.hpPerStr) + (enemy.stamina * enemy.hpPerSta);
        enemy.curHP = enemy.baseHP;

        //Calculate MP based on stats
        enemy.baseMP = Mathf.Round(enemy.intellect * enemy.mpPerInt);
        enemy.curMP = enemy.baseMP;

        //Calculate Attack based on stats
        enemy.baseATK = Mathf.Round((enemy.strength * enemy.atkPerStr) + (enemy.intellect * enemy.atkPerInt));
        enemy.curATK = enemy.baseATK;

        enemy.maxATK = enemy.baseATK + Random.Range(10, 50);
        enemy.minATK = enemy.baseATK;

        //Calculate HIT based on stats
        enemy.baseHit = Mathf.Round(enemy.dexterity * enemy.hitPerDex);
        enemy.curHit = enemy.baseHit;

        //Calculate dodge based on stats
        enemy.baseDodge = Mathf.Round(enemy.agility * enemy.dodgePerAgi);
        enemy.curDodge = enemy.baseDodge;

        //calculate def based on stats
        enemy.baseDEF = Mathf.Round(enemy.stamina * enemy.defPerSta);
        enemy.curDEF = enemy.baseDEF;

        //calculate critrate based on stats
        //enemy.curCRIT = enemy.baseCRIT;

        //calculate speed based on stats
        enemy.baseSpeed = Mathf.Round(enemy.agility * enemy.spdPerAgi);
        enemy.curSpeed = enemy.baseSpeed;

    }

    void PopulateSkilllist() //Take skills from the list of possible skills, calculate chances and spawn in according lists
    {
        for (int i = 0; i < PossibleSkills.Count; i++)
        {
            if(Random.Range(0, 100) < PossibleSkills[i].skillSpawnChance)
            {
                BaseAttack NewSkill = PossibleSkills[i].possibleSkill;
                //Debug.Log(NewSkill.attackType);
                if (NewSkill.attackType == "Spell")
                {
                    enemy.MagicAttacks.Add(NewSkill);
                }
                else
                {
                    enemy.attacks.Add(NewSkill);
                }
            }
        }
    }

    void DodgePopup()
    {
        enemyAnim.Play("Hurt"); // replace with step away animation later
        var go = Instantiate(FloatingText, transform.position, Quaternion.identity, transform); //tell that we dodged and no damage is dealt
        go.GetComponentInChildren<SpriteRenderer>().enabled = false;
        go.GetComponent<TextMeshPro>().fontSize = 2;
        go.GetComponent<TextMeshPro>().color = Color.white;
        go.GetComponent<TextMeshPro>().text = "DODGE";
    }


    void DamagePopup(bool isCritical, float DamageAmount, bool isHeal)
    {
        var go = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        if (isCritical == true)
        {   
            go.GetComponentInChildren<SpriteRenderer>().enabled = true;
            go.GetComponent<TextMeshPro>().fontSize = 6;
            go.GetComponent<TextMeshPro>().color = Color.red;
        }

        if (isHeal)
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

    void ResPopup(float ResHP)
    {
        var go2 = Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
        go2.GetComponentInChildren<SpriteRenderer>().enabled = false;
        go2.GetComponent<TextMeshPro>().color = Color.green;
        go2.GetComponent<TextMeshPro>().text = ResHP.ToString();
    }

    public void VampireHP(float damage)
    {
        float vampAmount = Mathf.Round((damage * 30) / 100);
        enemy.curHP += vampAmount;
        if(enemy.curHP > enemy.baseHP)
        {
            enemy.curHP = enemy.baseHP;
        }
        DamagePopup(false, vampAmount, true);
        Instantiate(HealVFX, transform.position, Quaternion.identity, transform);
        healthBar.SetSize(((enemy.curHP * 100) / enemy.baseHP) / 100);
    }

    public void SelfRessurect(int resChance, int resHP)
    {
        if (Random.Range(0, 100) < resChance)
        {
            enemy.curHP = Mathf.Round((enemy.baseHP / 100) * resHP);
            ResPopup(enemy.curHP);
            Instantiate(RessurectVFX, transform.position, Quaternion.identity, transform);
        }
        else
        {
            currentState = TurnState.DEAD;
            enemyAnim.Play("Die");
        }
    }
}

