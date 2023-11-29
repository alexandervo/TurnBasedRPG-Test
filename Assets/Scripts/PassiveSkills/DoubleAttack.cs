using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : PassiveSkill
{
    public DoubleAttack()
    {
        skillName = "Double Attack";
        skillLevel = 3;
        skillDescription = "Has a certain chance to perform 2 attacks while using basic attacks. Decreases base attack by 25%";
    }

    public bool ImplementDoubleAttack()
    {
        if (skillLevel == 1)
        {
            if(Random.Range(0, 100) < 15)
            {
                return true;
            }
        }
        if (skillLevel == 2)
        {
            if (Random.Range(0, 100) < 25)
            {
                return true;
            }
        }
        if (skillLevel == 3)
        {
            if (Random.Range(0, 100) < 35)
            {
                return true;
            }
        }
        return false;
    }

}
