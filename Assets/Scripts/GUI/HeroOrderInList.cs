using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroOrderInList : MonoBehaviour
{
    public int listOrder;
    public TMP_Text heroname;

    public void TransmitNumber()
    {
        GameObject.Find("HeroInformationPanel").GetComponent<HeroInfoInterface>().EnableHeroEditing(listOrder);
    }

    public void SetColor()
    {
        heroname.color = Color.white;
    }
}
