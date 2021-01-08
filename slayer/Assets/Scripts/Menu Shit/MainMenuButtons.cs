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

    public void OnClickOptions()
    {
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
}
