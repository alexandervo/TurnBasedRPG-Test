using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveSkill : MonoBehaviour
{
    public string skillName;
    [Range (1,3)]public int skillLevel;
    public string skillDescription;
    public Animation skillVFX;
    public Sprite skillIcon;
    

    public void Start()
    {
        skillVFX = GetComponent<Animation>();
        skillIcon = GetComponent<Sprite>();
    }

}
