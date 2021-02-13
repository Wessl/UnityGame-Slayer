using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject weaponpanel;
    public GameObject optionsPanel;
    public GameObject levelPanel;
    public GameObject how2PlayPanel;

    public void OnClickPlay()
    {
        if (PlayerPrefs.GetInt("ChosenLevel") == 0)     // This is in case it's the first time you start up
        {
            PlayerPrefs.SetInt("ChosenLevel", 1);
        }
        SceneManager.LoadScene(PlayerPrefs.GetInt("ChosenLevel"));
    }

    public void OnClickPickLevel()
    {
        if (levelPanel.activeSelf)
        {
            levelPanel.SetActive(false);
        }
        else
        {
            levelPanel.SetActive(true);
        }
    }

    public void OnClickPickWeapon()
    {
        if (weaponpanel.activeSelf)
        {
            weaponpanel.SetActive(false);
        }
        else
        {
            weaponpanel.SetActive(true);
        }
       
    }

    public void OnClickHowToPlay()
    {
        if (how2PlayPanel.activeSelf)
        {
            how2PlayPanel.SetActive(false);
        }
        else
        {
            how2PlayPanel.SetActive(true);
        }
    }

    // Sound options is handled in a separate script: may be a bad idea?
    public void OnClickOptions()
    {
        // First: Close the other panels to remove potential panel conflicts
        weaponpanel.SetActive(false);
        levelPanel.SetActive(false);
        // Now enable options panel, and set up the toggles so they match the current options. 
        optionsPanel.SetActive(true);
        var toggles = optionsPanel.GetComponentsInChildren<Toggle>();
        var currentActiveSwingType = PlayerPrefs.GetInt("SwingType");
        if (currentActiveSwingType == 0)
        {
            toggles[0].isOn = true;
            toggles[1].isOn = false;
        }
        else
        {
            toggles[0].isOn = false;
            toggles[1].isOn = true;
        }
    }

    public void OnClickCloseGame()
    {
        Debug.Log("Game shutting down, thanks for playing. Beep boop.");
        Application.Quit();
    }
}
