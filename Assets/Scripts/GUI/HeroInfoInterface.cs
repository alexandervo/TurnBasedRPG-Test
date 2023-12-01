using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class HeroInfoInterface : MonoBehaviour
{
    [SerializeField] GameObject HeroInfoPanelGameObject;
    [SerializeField] GameObject TestHero;

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

    public int newStr;
    public int newInt;
    public int newDex;
    public int newAgi;
    public int newSta;


    private void Start()
    {
        UpdateStats();
    }


    public void IncrStr()
    {
        if (TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints > 0)
        {
            TestHero.GetComponent<HeroStateMachine>().hero.strength++;
            TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints--;
            UpdateStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrStr()
    {
        TestHero.GetComponent<HeroStateMachine>().hero.strength--;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints++;
        UpdateStats();
    }


    public void IncrInt()
    {
        if (TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints > 0)
        {
            TestHero.GetComponent<HeroStateMachine>().hero.intellect++;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints--;
        UpdateStats();
        }
            else
            {
                Debug.Log("You don't have enough statpoints to destribute!");
            }
    }

    public void DecrInt()
    {
        TestHero.GetComponent<HeroStateMachine>().hero.intellect--;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints++;
        UpdateStats();
    }


    public void IncrDex()
    {
        if (TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints > 0)
        {
            TestHero.GetComponent<HeroStateMachine>().hero.dexterity++;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints--;
        UpdateStats();
        }
        else
        {
                    Debug.Log("You don't have enough statpoints to destribute!");
        }
     }

    public void DecrDex()
    {
        TestHero.GetComponent<HeroStateMachine>().hero.dexterity--;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints++;
        UpdateStats();
    }

    public void IncrAgi()
    {
        if (TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints > 0)
        {
            TestHero.GetComponent<HeroStateMachine>().hero.agility++;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints--;
        UpdateStats();
        }
        else
            {
                Debug.Log("You don't have enough statpoints to destribute!");
            }
        }

    public void DecrAgi()
    {
        TestHero.GetComponent<HeroStateMachine>().hero.agility--;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints++;
        UpdateStats();
    }

    public void IncrSta()
    {
        if(TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints > 0)
        {
            TestHero.GetComponent<HeroStateMachine>().hero.stamina++;
            TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints--;
            UpdateStats();
        }
        else
        {
            Debug.Log("You don't have enough statpoints to destribute!");
        }
    }

    public void DecrSta()
    {
        TestHero.GetComponent<HeroStateMachine>().hero.stamina--;
        TestHero.GetComponent<HeroStateMachine>().hero.unspentStatPoints++;
        UpdateStats();
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
}
