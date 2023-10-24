using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountdownTest : MonoBehaviour
{
    [SerializeField] TMP_Text countdownText;
    float currentCount = 0f;
    [SerializeField] float startingCount = 3f;


    void Start()
    {
        currentCount = startingCount;
    }

    // Update is called once per frame
    void Update()
    {
        currentCount -= 1 * Time.deltaTime;
        countdownText.text = currentCount.ToString("0");
        if (currentCount <= 0) 
        {
            countdownText.text = "Action Time!";
        }
    }
}
