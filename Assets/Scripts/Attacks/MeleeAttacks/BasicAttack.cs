using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : BaseAttack
{
    public BasicAttack()
    {
        attackName = "Basic Attack";
        attackDescription = "Simple melee attack with no modifications";
        attackType = "Melee";
        minDamage = 0;
        maxDamage = 0;
        attackDamage = 0;
        attackCost = 0;
    }

}
