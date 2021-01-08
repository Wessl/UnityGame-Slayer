using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGetNewWeaponOptions : MonoBehaviour
{
    public void OnEquipNewWeapon()
    {
        // Equip weapon by setting ChosenWeapon PlayerPrefs to the current level which corresponds to the buildindex of the scene
        var currentLevel = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("ChosenWeapon", currentLevel);
        OnClose();                                                          // Also close down the panel to indicate the action has been taken
    }
    
    // Closes down the panel New Weapon Options
    public void OnClose()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
