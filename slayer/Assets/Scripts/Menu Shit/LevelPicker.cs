using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPicker : MonoBehaviour
{
    // Assigned in editor - lazy "solution"
    public Text levelChosenText;

    public void OnClickThisLevel()
    {
        var levelIndex = transform.GetSiblingIndex() + 1;
        PlayerPrefs.SetInt("ChosenLevel", levelIndex);          // The level as an integer has to be += 1, since scene 0 is the Main Menu
        levelChosenText.text = "Level " + levelIndex + " picked!";
    }
}