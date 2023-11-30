using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseClass
{
    [Header("Main Info")]
    public string theName;
    //public string displayName;

    public Level level;
    //public int requiredExp;

    [Header("Main Attributes")]
    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;

    public float baseMATK = 50;
    public float curMATK = 50;

    public float maxATK;
    public float minATK;

    public float baseDEF;
    public float curDEF;

    public float baseCRIT = 0.25f;
    public float curCRIT = 0.25f;

    public float critDamage = 1.5f;

    public float baseSpeed;
    public float curSpeed;

    public float curDodge;
    public float baseDodge;

    public float baseHit;
    public float curHit;

    [Header("Stats")]
    public int strength;
    public int intellect;
    public int dexterity;
    public int agility;
    public int stamina;

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

    //[Header("Abilities")]
    public List<BaseAttack> attacks = new List<BaseAttack>();
    public List<BaseAttack> MagicAttacks = new List<BaseAttack>();

}
