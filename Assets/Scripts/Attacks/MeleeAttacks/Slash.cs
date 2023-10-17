using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack
{
    public Slash()
    {
        attackName = "Slash";
        attackDescription = "Slashes at enemy";
        attackDamage = 10f;
        attackCost = 0;
    }
}
