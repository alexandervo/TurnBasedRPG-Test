using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1Spell : BaseAttack
{
    public Fire1Spell() 
    {
        attackName = "Fire 1";
        attackDescription = "Basic fire spell";
        attackType = "Spell";
        minDamage = 5f;
        maxDamage = 15f;
        attackDamage = 15f;
        attackCost = 10f;
    }
}
