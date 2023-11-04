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

    public float baseDEF;
    public float curDEF;

    public float baseCRIT;
    public float curCRIT;

    public float critDamage = 1.5f;

    public List<BaseAttack> attacks = new List<BaseAttack>();
}
