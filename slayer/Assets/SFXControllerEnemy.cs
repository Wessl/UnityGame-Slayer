using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SFXControllerEnemy : MonoBehaviour
{
    [Header(" THIS CLASS NEEDS TO BE PRESENT IN THE SCENE WHERE SOUND EFFECTS ARE USED ")]
    public AudioClip hitSoundEffect;
    public AudioClip secondaryHitSoundEffect;
    public AudioSource audio;

    public void PlayGetHitSfx()
    {
        audio.PlayOneShot(hitSoundEffect);
    }

    public void PlaySecondaryHitSfx()
    {
        audio.PlayOneShot(secondaryHitSoundEffect);
    }
}
