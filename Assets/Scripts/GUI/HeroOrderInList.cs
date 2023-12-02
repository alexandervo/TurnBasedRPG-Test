using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroOrderInList : MonoBehaviour
{
    public int listOrder;

    public void TransmitNumber()
    {
        GameObject.Find("HeroInformationPanel").GetComponent<HeroInfoInterface>().EnableHeroEditing(listOrder);
    }
}
