using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsLister : MonoBehaviour
{
    public GameObject weaponsPanel;

    void Start()
    {
        var amountOfUnlockedWeapons = PlayerPrefs.GetInt("UnlockedWeapons");
        Image[] allChildren = weaponsPanel.GetComponentsInChildren<Image>();
        Debug.Log(PlayerPrefs.GetInt("UnlockedWeapons"));
        for (int i = allChildren.Length-1; i > amountOfUnlockedWeapons+1; i--)         
        {
            allChildren[i].gameObject.SetActive(false);
        }
    }
}
