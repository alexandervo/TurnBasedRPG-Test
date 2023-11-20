using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing : BaseAttack
{
    public HammerSwing()
    {
        attackName = "Hammer Swing";
        attackDescription = "Swings hammer at enemy";
        attackType = "Melee";
        minDamage = 10f;
        maxDamage = 25f;
        attackDamage = 25f;
        attackCost = 0;
    }
}
