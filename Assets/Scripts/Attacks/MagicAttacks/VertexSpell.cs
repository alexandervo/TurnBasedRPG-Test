using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexSpell : BaseAttack
{
    public VertexSpell() 
    {
        attackName = "Vertex";
        attackDescription = "Mass electric spell";
        attackType = "Spell";
        attackTargets = 3;
        minDamage = 5f;
        maxDamage = 15f;
        attackDamage = 15f;
        attackCost = 10f;
    }
}
