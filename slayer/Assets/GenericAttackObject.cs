using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAttackObject : MonoBehaviour
{
    
    [SerializeField] private string flavorText;
    public string GETFlavorText()
    {
        return flavorText;
    }
}
