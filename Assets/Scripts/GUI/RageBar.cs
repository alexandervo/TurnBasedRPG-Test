using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBar : MonoBehaviour
{
    private Transform bar;
    private void Awake()
    {
        bar = transform.Find("Bar");
    }

    public void SetRageBarSize(float sizeNormalized)
    {
            bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
