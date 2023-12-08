using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BaseHero : BaseClass
{
    //public int strength;
    //public int intellect;
    //public int dexterity;
    //public int agility;
    //public int stamina;

    [Header("Statpoints")]
    public int unspentStatPoints;

    [Header("Secondary Attributes")]
    public float maxRage = 150f;
    public float curRage = 0f;

    [Header("Statpoint growth")]
    public int statpointsPerLevel = 5;
    public int statIncreasePerLevel = 1;

    [Header("Avatar")]
    public GameObject heroAvatar;

}
