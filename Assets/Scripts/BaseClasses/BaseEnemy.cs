using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy : BaseClass
{
     public enum Type
    {
        NORMAL,
        STRONG,
        ELITE,
        MINIBOSS,
        MVP,
        BABY
    }

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        SUPERRARE
    }

    public Type EnemyType;
    public Rarity rarity;

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

}
