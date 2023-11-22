using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBar : MonoBehaviour
{
    private Transform rbar;
    private void Awake()
    {
        rbar = transform.Find("Bar");
    }

    public void SetRageBarSize(float sizeNormalized)
    {
        rbar.localScale = new Vector3 (sizeNormalized, 1f);
    }
}
