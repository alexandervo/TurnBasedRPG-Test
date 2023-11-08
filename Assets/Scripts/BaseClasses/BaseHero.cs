using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHero : BaseClass
{
    //public int strength;
    //public int intellect;
    //public int dexterity;
    //public int agility;
    //public int stamina;

    public float maxRage = 150f;
    public float curRage = 0f;

    public List<BaseAttack> MagicAttacks = new List<BaseAttack>();

}
