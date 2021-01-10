using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnLoseMenuOptions : MonoBehaviour
{
    // play again
    public void OnPlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenu()
    {
        // When you go back to the main menu, save the level you are on
        PlayerPrefs.SetInt("ChosenLevel", SceneManager.GetActiveScene().buildIndex);
        // Load scene 0 which is the main menu scene
        SceneManager.LoadScene(0);
    }
}
