using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison1Spell : BaseAttack
{
    public Poison1Spell()
    {
        attackName = "Poison1";
        attackDescription = "Basic poison attack";
        attackType = "Ranged";
        minDamage = 5f;
        maxDamage = 15f;
        attackDamage = 15f;
        attackCost = 8f;
    }
}
