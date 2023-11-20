using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack
{
    public Slash()
    {
        attackName = "Slash";
        attackDescription = "Slashes at enemy";
        attackType = "Melee";
        minDamage = 7f;
        maxDamage = 23f;
        attackDamage = 23f;
        attackCost = 0;
    }
}
