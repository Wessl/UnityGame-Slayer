using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is used to count and format the time passed in each scene (level)
public class Timer : MonoBehaviour
{
    public Text timeText;               // Set via editor
    
    private float timePassed;           // The time that has passed since beginning

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        DisplayTime(timePassed);
    }
    void DisplayTime(float timeToDisplay)
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
    }

    public string GetTimePassedAsString()
    {
        float minutes = Mathf.FloorToInt(timePassed / 60); 
        float seconds = Mathf.FloorToInt(timePassed % 60);

        var returnString = string.Format("{0:00}:{1:00}", minutes, seconds);
        return returnString;
    }
    
}
