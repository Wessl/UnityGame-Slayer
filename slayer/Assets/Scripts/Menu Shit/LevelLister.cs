using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLister : MonoBehaviour
{
    public GameObject levelPanel;

    // Only display the levels in the level picker that have been unlocked by the player
    void Start()
    {
        var amountOfUnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels");
        Image[] allChildren = levelPanel.GetComponentsInChildren<Image>();
        Debug.Log("Unlocked levels: " + PlayerPrefs.GetInt("UnlockedLevels"));
        for (int i = allChildren.Length-1; i > amountOfUnlockedLevels+1; i--)         
        {
            allChildren[i].gameObject.SetActive(false);
        }
    }
}