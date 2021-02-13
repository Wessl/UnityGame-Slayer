using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimePlayerHandlePlayerPrefs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // If it's the first time someone plays the game, set up some preferences to default values
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetFloat("musicSliderValue", 0.25f);
            PlayerPrefs.SetFloat("sfxSliderValue", 0.25f);
            PlayerPrefs.SetInt("SwingType", 1);
            PlayerPrefs.SetInt("FirstTime", 1);
        }
    }
}
