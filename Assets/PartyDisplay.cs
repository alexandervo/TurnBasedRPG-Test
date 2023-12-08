using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartyDisplay : MonoBehaviour
{
    public List<GameObject> partyMembers = new List<GameObject>();
    public List<Transform> avatarPlaceholders = new List<Transform>();
    [SerializeField] private GameObject positionFrame;
    [SerializeField] private GameObject avatarPanel;
    private GameObject myAv;


    // Start is called before the first frame update
    void Start()
    {
        partyMembers = GameManager.instance.battleHeroes;

        for (int i = 0; i < partyMembers.Count; i++)
        {
            GameObject av = partyMembers[i].GetComponent<HeroStateMachine>().hero.heroAvatar;
            myAv = Instantiate(av) as GameObject;
            myAv.transform.SetParent(avatarPlaceholders[i], false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
