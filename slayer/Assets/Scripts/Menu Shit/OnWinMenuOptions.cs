using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnWinMenuOptions : MonoBehaviour
{
    // play next level
    public void OnPlayNextLevel()
    {
        var chosenLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(chosenLevel);    // relies on all levels being in order in build settings
        PlayerPrefs.SetInt("ChosenLevel", chosenLevel);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
