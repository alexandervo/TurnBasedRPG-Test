using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing : BaseAttack
{
    public HammerSwing()
    {
        attackName = "Hammer Swing";
        attackDescription = "Swings hammer at enemy";
        attackDamage = 15f;
        attackCost = 0;
    }
}
