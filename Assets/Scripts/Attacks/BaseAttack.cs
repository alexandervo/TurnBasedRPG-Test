using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{
    public string attackName;
    public string attackDescription;
    public int attackLevel;
    public string attackType;
    public int attackTargets;
    public GameObject attackVFX;
    public float minDamage;
    public float maxDamage;
    public float attackDamage; //calculate the end damage
    public float attackCost; //if it's a spell, requires MP


}
