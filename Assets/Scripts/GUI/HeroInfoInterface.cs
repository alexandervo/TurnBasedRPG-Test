using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class HeroInfoInterface : MonoBehaviour
{
    [SerializeField] GameObject HeroInfoPanelGameObject;
    [SerializeField] GameObject TestHero;

    //Main information and stat display
    [Header("Display Information")]
    public TMP_Text heroListName;
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
    public int curStr;
    public int curInt;
    public int curDex;
    public int curAgi;
    public int curSta;
    public int curStatpts;

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

    //Not used anywhere? Delete bit later
    //public float curHP;
    //public float curMP;
    //public float curAtk;
    //public float curMatk;
    //public float curdDef;

    //public float curCrit;
    //public float curDodge;
    //public float curHit;
    //public float curSpeed;

    private void Start()
    {
        UpdateStats();
        UpdateDisplayStats();
        curStr = TestHero.GetComponent<HeroStateMachine>().hero.strength;
        curInt = TestHero.GetComponent<HeroStateMachine>().hero.intellect;
        curDex = TestHero.GetComponent<HeroStateMachine>().hero.dexterity;
        curAgi = TestHero.GetComponent<HeroStateMachine>().hero.agility;
        curSta = TestHero.GetComponent<HeroStateMachine>().hero.stamina;

        curStatpts = TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints;


        //Not used anywhere? Delete bit later
        //curHP = TestHero.GetComponent<HeroStateMachine>().hero.baseHP;
        //curMP = TestHero.GetComponent<HeroStateMachine>().hero.baseMP;
        //curAtk = TestHero.GetComponent<HeroStateMachine>().hero.baseATK;
        //curMatk = TestHero.GetComponent<HeroStateMachine>().hero.baseMATK;
        //curdDef = TestHero.GetComponent<HeroStateMachine>().hero.baseDEF;

        //curCrit = TestHero.GetComponent<HeroStateMachine>().hero.baseCRIT;

        //curDodge = TestHero.GetComponent<HeroStateMachine>().hero.baseDodge;
        //curHit = TestHero.GetComponent<HeroStateMachine>().hero.baseHit;
        //curSpeed = TestHero.GetComponent<HeroStateMachine>().hero.baseSpeed;
}

    public void DecrStatpointDisplay()
    {
        heroStatptsPre.text = "-" + addedStatpts.ToString();
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
        heroListName.text = TestHero.GetComponent<HeroStateMachine>().hero.theName;

        heroName.text = TestHero.GetComponent<HeroStateMachine>().hero.theName;
        heroLevel.text = TestHero.GetComponent<HeroStateMachine>().hero.level.currentlevel.ToString();

        heroHP.text = TestHero.GetComponent<HeroStateMachine>().hero.curHP.ToString() + " / " + TestHero.GetComponent<HeroStateMachine>().hero.baseHP.ToString();
        heroMP.text = TestHero.GetComponent<HeroStateMachine>().hero.curMP.ToString() + " / " + TestHero.GetComponent<HeroStateMachine>().hero.baseMP.ToString();

        heroAtk.text = TestHero.GetComponent<HeroStateMachine>().hero.curATK.ToString();
        heroMatk.text = TestHero.GetComponent<HeroStateMachine>().hero.curMATK.ToString();
        heroDef.text = TestHero.GetComponent<HeroStateMachine>().hero.curDEF.ToString();

        heroDodge.text = TestHero.GetComponent<HeroStateMachine>().hero.curDodge.ToString();
        heroHit.text = TestHero.GetComponent<HeroStateMachine>().hero.curHit.ToString();
        heroCrit.text = TestHero.GetComponent<HeroStateMachine>().hero.curCRIT.ToString();
        heroSpeed.text = TestHero.GetComponent<HeroStateMachine>().hero.curSpeed.ToString();

        heroStr.text = TestHero.GetComponent<HeroStateMachine>().hero.strength.ToString();
        heroInt.text = TestHero.GetComponent<HeroStateMachine>().hero.intellect.ToString();
        heroDex.text = TestHero.GetComponent<HeroStateMachine>().hero.dexterity.ToString();
        heroAgi.text = TestHero.GetComponent<HeroStateMachine>().hero.agility.ToString();
        heroSta.text = TestHero.GetComponent<HeroStateMachine>().hero.stamina.ToString();

        heroStatpts.text = TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints.ToString();

        heroExp.text = TestHero.GetComponent<HeroStateMachine>().hero.level.experience.ToString() + " / " + TestHero.GetComponent<HeroStateMachine>().hero.level.requiredExp.ToString();
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
        if (TestHero != null)
        {
            TestHero.GetComponent<HeroStateMachine>().hero.strength += addedStr;
            TestHero.GetComponent<HeroStateMachine>().hero.intellect += addedInt;
            TestHero.GetComponent<HeroStateMachine>().hero.dexterity += addedDex;
            TestHero.GetComponent<HeroStateMachine>().hero.agility += addedAgi;
            TestHero.GetComponent<HeroStateMachine>().hero.stamina += addedSta;

            TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints -= addedStatpts;

            TestHero.GetComponent<HeroStateMachine>().hero.baseHP += addedHP;
            TestHero.GetComponent<HeroStateMachine>().hero.curHP = TestHero.GetComponent<HeroStateMachine>().hero.baseHP;
            TestHero.GetComponent<HeroStateMachine>().hero.baseMP += addedMP;
            TestHero.GetComponent<HeroStateMachine>().hero.curMP = TestHero.GetComponent<HeroStateMachine>().hero.baseMP;


            TestHero.GetComponent<HeroStateMachine>().hero.baseATK += addedAtk;
            TestHero.GetComponent<HeroStateMachine>().hero.curATK = TestHero.GetComponent<HeroStateMachine>().hero.baseATK;
            TestHero.GetComponent<HeroStateMachine>().hero.baseMATK += addedMatk;
            TestHero.GetComponent<HeroStateMachine>().hero.curMATK = TestHero.GetComponent<HeroStateMachine>().hero.baseMATK;
            TestHero.GetComponent<HeroStateMachine>().hero.baseDEF += addedDef;
            TestHero.GetComponent<HeroStateMachine>().hero.curDEF = TestHero.GetComponent<HeroStateMachine>().hero.baseDEF;

            TestHero.GetComponent<HeroStateMachine>().hero.baseCRIT += addedCrit;
            TestHero.GetComponent<HeroStateMachine>().hero.curCRIT = TestHero.GetComponent<HeroStateMachine>().hero.baseCRIT;
            TestHero.GetComponent<HeroStateMachine>().hero.baseDodge += addedDodge;
            TestHero.GetComponent<HeroStateMachine>().hero.curDodge = TestHero.GetComponent<HeroStateMachine>().hero.baseDodge;
            TestHero.GetComponent<HeroStateMachine>().hero.baseHit += addedHit;
            TestHero.GetComponent<HeroStateMachine>().hero.curHit = TestHero.GetComponent<HeroStateMachine>().hero.baseHit;
            TestHero.GetComponent<HeroStateMachine>().hero.baseSpeed += addedSpeed;
            TestHero.GetComponent<HeroStateMachine>().hero.curSpeed = TestHero.GetComponent<HeroStateMachine>().hero.baseSpeed;

            Clean();
            UpdateStats();
            UpdateDisplayStats();
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
}
