using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySelectButton : MonoBehaviour
{
    public GameObject AllyPrefab;

    public void SelectAlly()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ().Input6(AllyPrefab); //save input ally prefab
    }

    public void HideSelector()
    {
            AllyPrefab.transform.Find("Selector").gameObject.SetActive(false);      
    }
    public void ShowSelector()
    {
        AllyPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }
}
