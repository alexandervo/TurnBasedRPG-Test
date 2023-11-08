using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseClass
{
    public string theName;

    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;

    public float maxATK;
    public float minATK;

    public float baseDEF;
    public float curDEF;

    public float baseCRIT = 0.25f;
    public float curCRIT = 0.25f;

    public float critDamage = 1.5f;

    public int baseSpeed = 15;
    public int curSpeed = 15;

    public float curDodge = 100f;
    public float curHit = 50f;

    //public float hpPerStr = 10;
    //public float atkPerStr = 5;
    //public float mpPerInt = 5;
    //public float atkPerInt = 5;
    //public float spdPerAgi = 2;
    //public float evasionPerAgi = 2;
    //public float hitPerDex = 2;
    //public float atkPerDex = 2;
    //public float hpPerSta = 15;
    //public float defPerSta = 5;


    public List<BaseAttack> attacks = new List<BaseAttack>();

}
