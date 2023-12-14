using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassHealSpell : BaseAttack
{
        public MassHealSpell()
        {
            attackName = "Mass Heal";
            attackDescription = "Healing few targets.";
            attackType = "Spell";
            isAttack = false;
            attackTargets = 3;
            minDamage = 5f;
            maxDamage = 15f;
            attackDamage = 15f;
            attackCost = 50f;
        }
    
}
