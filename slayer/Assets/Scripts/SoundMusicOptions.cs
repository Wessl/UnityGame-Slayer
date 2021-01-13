using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundMusicOptions : MonoBehaviour
{
    private Slider _soundEffectSlider;
    private Slider _musicSlider;

    private float _sfxValue;
    private float _musicValue;
    // Start is called before the first frame update
    void Start()
    {
        _soundEffectSlider = GameObject.FindWithTag("SFXSlider").GetComponent<Slider>();
        _musicSlider       = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();
        if (PlayerPrefs.HasKey("sfxSliderValue"))
        {
            _soundEffectSlider.value = PlayerPrefs.GetFloat("sfxSliderValue");
            _sfxValue   =  _soundEffectSlider.value;
        }

        if (PlayerPrefs.HasKey("musicSliderValue"))
        {
            _musicSlider.value       = PlayerPrefs.GetFloat("musicSliderValue");
            _musicValue =  _musicSlider.value;
        }

    }

    void Update()
    {
        if (Math.Abs(_soundEffectSlider.value - _sfxValue) > Int32.MinValue) // This is to check if something has changed, using Int32.MinValue (0x80000000) to account for floating point comparison errors
        {
            // Value changed
            _sfxValue   = _soundEffectSlider.value;
            PlayerPrefs.SetFloat("sfxSliderValue", _sfxValue);
        }

        if (Math.Abs(_musicSlider.value - _musicValue) > Int32.MinValue)
        {
            // Value changed
            _musicValue = _musicSlider.value;
            PlayerPrefs.SetFloat("musicSliderValue", _musicValue);
            GameObject.FindWithTag("MusicAudioSource").GetComponent<MusicVolume>().OnChangeVolume();
        }
    }
}
