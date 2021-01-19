using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        string t = "Level " + level;
        GetComponent<Text>().text = t;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
