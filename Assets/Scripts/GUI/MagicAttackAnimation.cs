using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackAnimation : MonoBehaviour
{
    public float DestroyTime = 1f;
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
