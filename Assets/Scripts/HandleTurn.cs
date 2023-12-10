using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn
{
    public string Attacker; //name of attacker
    public float attackersSpeed; //speed of the attacker
    public string Type; // 
    public GameObject AttackersGameObject; //who attacks
    public List<GameObject> AttackersTarget = new List<GameObject>(); // who is attacked

    //which type of attack is performed
    public BaseAttack choosenAttack;
}
