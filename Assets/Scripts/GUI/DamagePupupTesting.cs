using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class DamagePupupTesting : MonoBehaviour
{

    private void Start()
    {
        DamagePopup.Create(Vector3.zero, 300);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DamagePopup.Create(UtilsClass.GetMouseWorldPosition(), 100);
        }
    }
}
