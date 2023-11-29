using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UIElements;

public class HeroInfoInterface : MonoBehaviour
{
    public List<GameObject> HeroList = new List<GameObject>();
    public GameObject HeroInList;
    //public GameObject setname;
 
    void Start()
    {
        
        for (var i = 0; i < HeroList.Count; i++)
        {

            HeroInList = Instantiate(HeroInList);
                       
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
