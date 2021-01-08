using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptions : MonoBehaviour
{
    // Toggle number one, which is set to True if the player wants swing type 0 - towards transform.forward
    public void OnClickToggleWs1()
    {
        PlayerPrefs.SetInt("SwingType", 0);
    }
    
    // Toggle number two, True if player wants swing type 1 - towards mouse pointer position
    public void OnClickToggleWs2()
    {
        PlayerPrefs.SetInt("SwingType", 1);
    }

    // Close this panel by disabling the parent gameobject
    public void OnClose()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
