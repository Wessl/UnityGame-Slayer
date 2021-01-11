using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SFXControllerEnemy : MonoBehaviour
{
    [Header("MUST TO BE PRESENT IN SCENES WHERE SFX ARE USED")]

    public AudioClip metalHitOne;
    public AudioClip metalHitTwo;
    public AudioClip slimeHit;
    public AudioClip woodHit;
    public AudioClip stoneHit;
    public AudioSource audio;

    private float sfxVolume;

    void Start()
    {
        sfxVolume = PlayerPrefs.GetFloat("sfxSliderValue");
    }

    public void MetalBallHitOne()
    {
        audio.PlayOneShot(metalHitOne, sfxVolume);
    }

    public void MetalBallHitTwo()
    {
        audio.PlayOneShot(metalHitTwo, sfxVolume);
    }

    public void SlimeHit()
    {
        audio.PlayOneShot(slimeHit, sfxVolume);
    }

    public void WoodHit()
    {
        audio.PlayOneShot(woodHit, sfxVolume);
    }

    public void StoneHit()
    {
        audio.PlayOneShot(stoneHit, sfxVolume);   
    }
}
