using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vol = PlayerPrefs.GetFloat("musicSliderValue");
        var audioSource = GetComponent<AudioSource>();
        audioSource.volume = vol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeVolume()
    {
        var vol = PlayerPrefs.GetFloat("musicSliderValue");
        var audioSource = GetComponent<AudioSource>();
        audioSource.volume = vol;
    }
}
