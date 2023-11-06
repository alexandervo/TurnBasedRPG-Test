using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHero : BaseClass
{
    public int strength;
    public int intellect;
    public int dexterity;
    public int agility;
    public int stamina;

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

    public List<BaseAttack> MagicAttacks = new List<BaseAttack>();
    //public void OnValidate()
    //{
    //    baseHP = Mathf.Round(strength * hpPerStr);
    //    curHP = Mathf.Round(strength * hpPerStr);

    //    baseATK = Mathf.Round((strength * atkPerStr) + (dexterity * atkPerDex) + (intellect * atkPerInt));
    //    curATK = Mathf.Round((strength * atkPerStr) + (dexterity * atkPerDex) + (intellect * atkPerInt));

    //    maxATK = Mathf.Round((strength * atkPerStr) + (dexterity * atkPerDex) + (intellect * atkPerInt)) + Random.Range(5,15);
    //    minATK = Mathf.Round((strength * atkPerStr) + (dexterity * atkPerDex) + (intellect * atkPerInt));

    //    baseMP = Mathf.Round(intellect * mpPerInt);
    //    curMP = Mathf.Round(intellect * mpPerInt);

    //}

}
