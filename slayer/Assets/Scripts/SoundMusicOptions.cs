using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundMusicOptions : MonoBehaviour
{
    private Slider _soundEffectSlider;
    private Slider _musicSlider;

    private int _sfxValue;
    private int _musicValue;
    // Start is called before the first frame update
    void Start()
    {
        _soundEffectSlider = GameObject.FindWithTag("SFXSlider").GetComponent<Slider>();
        _musicSlider       = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();
        _soundEffectSlider.value = PlayerPrefs.GetInt("sfxSliderValue");
        _musicSlider.value       = PlayerPrefs.GetInt("musicSliderValue");
        _sfxValue   = (int) _soundEffectSlider.value;
        _musicValue = (int) _musicSlider.value;
    }

    void Update()
    {
        if (_soundEffectSlider.value != _sfxValue)  // The values from the sliders will always be integers
        {
            // Value changed
            _sfxValue   = (int) _soundEffectSlider.value;
            PlayerPrefs.SetInt("sfxSliderValue", _sfxValue);
        }

        if (_musicSlider.value != _musicValue)
        {
            // Value changed
            _musicValue = (int) _musicSlider.value;
            PlayerPrefs.SetInt("musicSliderValue", _musicValue);

        }
    }
}
