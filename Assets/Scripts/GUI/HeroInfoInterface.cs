using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HeroInfoInterface : MonoBehaviour
{
    [SerializeField] GameObject HeroInfoPanelGameObject;
    //[SerializeField] GameObject TestHero;
    [SerializeField] Transform HeroListDisplay;
    [SerializeField] GameObject button;

    public List<GameObject> heroList = new List<GameObject>();
    public List<Button> buttonList = new List<Button>();

    [Header("TEST")]
    [SerializeField] GameObject heroPrefab;

    //Main information and stat display
    [Header("Display Information")]
    //public TMP_Text heroListName;
    public TMP_Text heroSlots;

    public TMP_Text heroName;
    public TMP_Text heroLevel;

    public TMP_Text heroHP;
    public TMP_Text heroMP;
    public TMP_Text heroAtk;
    public TMP_Text heroMatk;
    public TMP_Text heroDef;

    public TMP_Text heroDodge;
    public TMP_Text heroHit;
    public TMP_Text heroCrit;
    public TMP_Text heroSpeed;

    public TMP_Text heroStr;
    public TMP_Text heroInt;
    public TMP_Text heroDex;
    public TMP_Text heroAgi;
    public TMP_Text heroSta;

    public TMP_Text heroStatpts;

    public TMP_Text heroLoyalty;
    public TMP_Text heroExp;

    //Stat preview display upon pre-levelling stats
    [Header("Stats preview text")]
    public TMP_Text heroHPpre;
    public TMP_Text heroMPpre;
    public TMP_Text heroAtkpre;
    public TMP_Text heroMatkpre;
    public TMP_Text heroDefpre;

    public TMP_Text heroDodgepre;
    public TMP_Text heroHitpre;
    public TMP_Text heroCritpre;
    public TMP_Text heroSpeedpre;

    public TMP_Text heroStrpre;
    public TMP_Text heroIntpre;
    public TMP_Text heroDexpre;
    public TMP_Text heroAgipre;
    public TMP_Text heroStapre;

    public TMP_Text heroStatptsPre;

    //Variables for stat pre-levelling
    [Header("Will be added")]
    public int addedStr;
    public int addedInt;
    public int addedDex;
    public int addedAgi;
    public int addedSta;

    public int addedStatpts;

    public float addedHP;
    public float addedMP;
    public float addedAtk;
    public float addedMatk;
    public float addedDef;

    public float addedCrit;
    public float addedDodge;
    public float addedHit;
    public float addedSpeed;

    [Header("Temporal display values")]
    //public int curStr;
    //public int curInt;
    //public int curDex;
    //public int curAgi;
    //public int curSta;
    public int curStatpts;

    public int curSlots;

    [Header("Attribute multipliers")]
    public float hpPerStr = 10;
    public float atkPerStr = 5;
    public float mpPerInt = 10;
    public float atkPerInt = 5;
    public float spdPerAgi = 2;
    public float dodgePerAgi = 3;
    public float hitPerDex = 2;
    public float atkPerDex = 2;
    public float hpPerSta = 25;
    public float defPerSta = 5;


    private void Awake()
    {
        heroPrefab = heroList[0];
        CreateCharNameButtons2();

    }

    private void Start()
    {
        UpdateStats();
        SetCurStats();
        UpdateDisplayStats();
        AssignClickEvents();
    }

    public void CreateCharNameButtons2()
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            GameObject Btn = Instantiate(button);
            Btn.GetComponentInChildren<TextMeshProUGUI>().text = heroList[i].GetComponent<HeroStateMachine>().hero.theName;
            Btn.GetComponent<HeroOrderInList>().listOrder = i;
            int a = i;
            Btn.GetComponent<Button>().onClick.AddListener(() => { EnableHeroEditing(a); });
            //Debug.Log("Iteration " + i);
            Btn.transform.SetParent(HeroListDisplay, false);
            buttonList.Add(Btn.GetComponent<Button>());
        }
    }


    private void AssignClickEvents()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int a = i;
            buttonList[i].GetComponent<Button>().onClick.AddListener(() => { EnableHeroEditing(a); });
        }
    }
    public void EnableHeroEditing(int a)
    {
        Debug.Log(a);
        heroPrefab = heroList[a];
        SetCurStats();
        UpdateStats();
        curStatpts = heroPrefab.GetComponent<HeroStateMachine>().hero.unspentStatPoints;
        Debug.Log(curStatpts);
    }

    public void DecrStatpointDisplay()
    {
        heroStatptsPre.text = "-" + addedStatpts.ToString();
    }

    void SetCurStats()
    {
        //curStr = heroPrefab.GetComponent<HeroStateMachine>().hero.strength;
        //curInt = heroPrefab.GetComponent<HeroStateMachine>().hero.intellect;
        //curDex = heroPrefab.GetComponent<HeroStateMachine>().hero.dexterity;
        //curAgi = heroPrefab.GetComponent<HeroStateMachine>().hero.agility;
        //curSta = heroPrefab.GetComponent<HeroStateMachine>().hero.stamina;

        
    }

    public void IncrStr()
    {
        if (curStatpts > 0)
        {
            addedStr++;
            curStatpts--;
            addedStatpts++;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrStr()
    {
        if(addedStr > 0)
        {
            addedStr--;
            curStatpts++;
            addedStatpts--;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You can't decrease your stat any lower!");
        }

    }


    public void IncrInt()
    {
        if (curStatpts > 0)
        {
            addedInt++;
            curStatpts--;
            addedStatpts++;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrInt()
    {
        if (addedInt > 0)
        {
            addedInt--;
            curStatpts++;
            addedStatpts--;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You can't decrease your stat any lower!");
        }
    }


    public void IncrDex()
    {
        if (curStatpts > 0)
        {
            addedDex++;
            curStatpts--;
            addedStatpts++;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrDex()
    {
        if (addedDex > 0)
        {
            addedDex--;
            curStatpts++;
            addedStatpts--;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You can't decrease your stat any lower!");
        }
    }

    public void IncrAgi()
    {
        if (curStatpts > 0)
        {
            addedAgi++;
            curStatpts--;
            addedStatpts++;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrAgi()
    {
        if (addedAgi > 0)
        {
            addedAgi--;
            curStatpts++;
            addedStatpts--;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You can't decrease your stat any lower!");
        }
    }

    public void IncrSta()
    {
        if (curStatpts > 0)
        {
            addedSta++;
            curStatpts--;
            addedStatpts++;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrSta()
    {
        if (addedSta > 0)
        {
            addedSta--;
            curStatpts++;
            addedStatpts--;
            DecrStatpointDisplay();
            CalculateStatBonus();
            UpdateDisplayStats();
        }
        else
        {
            Debug.Log("You can't decrease your stat any lower!");
        }
    }

    public void UpdateStats()
    {
        if (heroPrefab != null)
        {
            //heroListName.text = heroPrefab.GetComponent<HeroStateMachine>().hero.theName;
            heroSlots.text = heroList.Count.ToString() + " / " + heroList.Count.ToString();
            heroName.text = heroPrefab.GetComponent<HeroStateMachine>().hero.theName.ToString();
            heroLevel.text = heroPrefab.GetComponent<HeroStateMachine>().hero.level.currentlevel.ToString();

            heroHP.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curHP.ToString() + " / " + heroPrefab.GetComponent<HeroStateMachine>().hero.baseHP.ToString();
            heroMP.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curMP.ToString() + " / " + heroPrefab.GetComponent<HeroStateMachine>().hero.baseMP.ToString();

            heroAtk.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curATK.ToString();
            heroMatk.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curMATK.ToString();
            heroDef.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curDEF.ToString();

            heroDodge.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curDodge.ToString();
            heroHit.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curHit.ToString();
            heroCrit.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curCRIT.ToString();
            heroSpeed.text = heroPrefab.GetComponent<HeroStateMachine>().hero.curSpeed.ToString();

            heroStr.text = heroPrefab.GetComponent<HeroStateMachine>().hero.strength.ToString();
            heroInt.text = heroPrefab.GetComponent<HeroStateMachine>().hero.intellect.ToString();
            heroDex.text = heroPrefab.GetComponent<HeroStateMachine>().hero.dexterity.ToString();
            heroAgi.text = heroPrefab.GetComponent<HeroStateMachine>().hero.agility.ToString();
            heroSta.text = heroPrefab.GetComponent<HeroStateMachine>().hero.stamina.ToString();

            heroStatpts.text = heroPrefab.GetComponent<HeroStateMachine>().hero.unspentStatPoints.ToString();

            heroExp.text = heroPrefab.GetComponent<HeroStateMachine>().hero.level.experience.ToString() + " / " + heroPrefab.GetComponent<HeroStateMachine>().hero.level.requiredExp.ToString();
        }
    }

    public void UpdateDisplayStats()
    {
        if (addedHP > 0)
            heroHPpre.text = "+" + addedHP.ToString();
        else
            heroHPpre.text = "";

        if (addedMP > 0)
            heroMPpre.text = "+" + addedMP.ToString();
        else
            heroMPpre.text = "";

        if (addedAtk > 0)
            heroAtkpre.text = "+" + addedAtk.ToString();
        else
            heroAtkpre.text = "";

        if (addedMatk > 0)
            heroMatkpre.text = "+" + addedMatk.ToString();
        else
            heroMatkpre.text = "";

        if (addedDef > 0)
            heroDefpre.text = "+" + addedDef.ToString();
        else
            heroDefpre.text = "";

        if (addedDodge > 0)
            heroDodgepre.text = "+" + addedDodge.ToString();
        else
            heroDodgepre.text = "";

        if (addedHit > 0)
            heroHitpre.text = "+" + addedHit.ToString();
        else
            heroHitpre.text = "";

        if (addedCrit > 0)
            heroCritpre.text = "+" + addedCrit.ToString();
        else
            heroCritpre.text = "";

        if (addedSpeed > 0)
            heroSpeedpre.text = "+" + addedSpeed.ToString();
        else
            heroSpeedpre.text = "";

        if (addedStr > 0)
            heroStrpre.text = "+" + addedStr.ToString();
        else
            heroStrpre.text = "";

        if (addedInt > 0)
            heroIntpre.text = "+" + addedInt.ToString();
        else
            heroIntpre.text = "";

        if (addedDex > 0)
            heroDexpre.text = "+" + addedDex.ToString();
        else
            heroDexpre.text = "";

        if (addedAgi > 0)
            heroAgipre.text = "+" + addedAgi.ToString();
        else
            heroAgipre.text = "";

        if (addedSta > 0)
            heroStapre.text = "+" + addedSta.ToString();
        else
            heroStapre.text = "";

        if (addedStatpts > 0)
            heroStatptsPre.text = "-" + addedStatpts.ToString();
        else
            heroStatptsPre.text = "";

    }

    public void FireStatIncrease()
    {
        if (heroPrefab != null)
        {
            heroPrefab.GetComponent<HeroStateMachine>().hero.strength += addedStr;
            heroPrefab.GetComponent<HeroStateMachine>().hero.intellect += addedInt;
            heroPrefab.GetComponent<HeroStateMachine>().hero.dexterity += addedDex;
            heroPrefab.GetComponent<HeroStateMachine>().hero.agility += addedAgi;
            heroPrefab.GetComponent<HeroStateMachine>().hero.stamina += addedSta;

            heroPrefab.GetComponent<HeroStateMachine>().hero.unspentStatPoints -= addedStatpts;

            heroPrefab.GetComponent<HeroStateMachine>().hero.baseHP += addedHP;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curHP = heroPrefab.GetComponent<HeroStateMachine>().hero.baseHP;
            heroPrefab.GetComponent<HeroStateMachine>().hero.baseMP += addedMP;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curMP = heroPrefab.GetComponent<HeroStateMachine>().hero.baseMP;


            heroPrefab.GetComponent<HeroStateMachine>().hero.baseATK += addedAtk;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curATK = heroPrefab.GetComponent<HeroStateMachine>().hero.baseATK;
            heroPrefab.GetComponent<HeroStateMachine>().hero.baseMATK += addedMatk;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curMATK = heroPrefab.GetComponent<HeroStateMachine>().hero.baseMATK;
            heroPrefab.GetComponent<HeroStateMachine>().hero.baseDEF += addedDef;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curDEF = heroPrefab.GetComponent<HeroStateMachine>().hero.baseDEF;

            heroPrefab.GetComponent<HeroStateMachine>().hero.baseCRIT += addedCrit;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curCRIT = heroPrefab.GetComponent<HeroStateMachine>().hero.baseCRIT;
            heroPrefab.GetComponent<HeroStateMachine>().hero.baseDodge += addedDodge;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curDodge = heroPrefab.GetComponent<HeroStateMachine>().hero.baseDodge;
            heroPrefab.GetComponent<HeroStateMachine>().hero.baseHit += addedHit;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curHit = heroPrefab.GetComponent<HeroStateMachine>().hero.baseHit;
            heroPrefab.GetComponent<HeroStateMachine>().hero.baseSpeed += addedSpeed;
            heroPrefab.GetComponent<HeroStateMachine>().hero.curSpeed = heroPrefab.GetComponent<HeroStateMachine>().hero.baseSpeed;

            Clean();

        }
    }

    public void Clean()
    {
        addedStr = 0;
        addedInt = 0;
        addedDex = 0;
        addedAgi = 0;
        addedSta = 0;

        addedStatpts = 0;
        addedHP = 0;
        addedMP = 0;
        addedAtk = 0;
        addedMatk = 0;
        addedDef = 0;
        addedCrit = 0;
        addedDodge = 0;
        addedHit = 0;
        addedSpeed = 0;

        UpdateStats();
        UpdateDisplayStats();
    }

    void CalculateStatBonus()
    {
        //Calculate HP based on Stats
        addedHP = Mathf.Round(addedStr * hpPerStr) + (addedSta * hpPerSta);

        //Calculate MP based on stats
        addedMP = Mathf.Round(addedInt * mpPerInt);

        //Calculate Attack based on stats
        addedAtk = Mathf.Round((addedStr * atkPerStr) + (addedInt * atkPerInt) / 10);

        //Calculate magic Attack based on stats
        addedMatk = Mathf.Round((addedStr * atkPerStr) + (addedInt * atkPerInt) / 10);

        //Calculate HIT based on stats
        addedHit = Mathf.Round(addedDex * hitPerDex);

        //Calculate dodge based on stats
        addedDodge = Mathf.Round(addedAgi * dodgePerAgi);

        //calculate def based on stats
        addedDef = Mathf.Round((addedSta * defPerSta) / 2);

        //calculate critrate based on stats
        addedCrit = 0;

        //calculate speed based on stats
        addedSpeed = Mathf.Round(addedAgi * spdPerAgi);
    }

    void OnDisable()
    {
        Clean();

    }

}
