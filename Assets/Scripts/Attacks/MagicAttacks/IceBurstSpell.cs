using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBurstSpell : BaseAttack
{
    public IceBurstSpell()
    {
        attackName = "IceBurst";
        attackDescription = "Mass magig ice attack. Attacks " + attackTargets + " targets.";
        attackType = "Spell";
        attackLevel = 3;
        attackTargets = 3;
        minDamage = 5f;
        maxDamage = 15f;
        attackDamage = 15f;
        attackCost = 8f;
    }

    private void Start()
    {
        attackTargets = attackLevel;
    }
}
