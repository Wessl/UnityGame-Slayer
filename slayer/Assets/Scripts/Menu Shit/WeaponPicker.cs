using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPicker : MonoBehaviour
{
    public Text weaponChosenText;

    [SerializeField] private string weaponName = "DefaultWeaponName";
    public void OnClickThisWeapon()
    {
        var weaponIndex = transform.GetSiblingIndex();
        PlayerPrefs.SetInt("ChosenWeapon", weaponIndex);
        weaponChosenText.text = weaponName + " picked!";
    }
}
