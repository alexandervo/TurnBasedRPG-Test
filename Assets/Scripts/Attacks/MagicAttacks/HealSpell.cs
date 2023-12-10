using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : BaseAttack
{
        public HealSpell()
        {
            attackName = "Heal";
            attackDescription = "Healing one or few targets.";
            attackType = "Spell";
            isAttack = false;
            attackTargets = 1;
            minDamage = 5f;
            maxDamage = 15f;
            attackDamage = 15f;
            attackCost = 50f;
        }
    
}
