using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip victorySong;
    // Start is called before the first frame update
    void Start()
    {
        var vol = PlayerPrefs.GetFloat("musicSliderValue");
        var audioSource = GetComponent<AudioSource>();
        audioSource.volume = vol;
    }

    public void OnChangeVolume()
    {
        var vol = PlayerPrefs.GetFloat("musicSliderValue");
        var audioSource = GetComponent<AudioSource>();
        audioSource.volume = vol;
    }

    // Win the current level, play victory song and disable looping. 
    public void OnLevelVictory()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = victorySong;
        audioSource.volume *= 2f;     // Kinda dirty, but the volume of this clip is a bit lower than the others
        audioSource.PlayOneShot(victorySong);
        audioSource.loop = false;
    }
}
