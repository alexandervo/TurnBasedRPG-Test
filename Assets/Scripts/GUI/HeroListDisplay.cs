using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroListDisplay : MonoBehaviour
{
    public GameObject heroInfoPanel;
    public void GetFunction()
    {
        heroInfoPanel.GetComponent<HeroInfoInterface>().Clean();
    }
}

