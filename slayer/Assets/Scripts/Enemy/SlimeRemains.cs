using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class SlimeRemains : MonoBehaviour
{
    // How long this object should be alive before being destroyed
    [SerializeField] public float duration;
    private Light2D _myLight;

    // Need to get component on Awake, since SetLightSettings() is called the same frame as this is instantiated, which is before start
    void Awake()
    {
        _myLight = GetComponent<Light2D>(); 
    }
    void Start()
    {
        StartCoroutine(Life());
    }

    // Waits for 'duration' seconds before destroying self
    private IEnumerator Life()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void SetLightSettings(Light2D l)
    {
        _myLight.color = l.color;
        _myLight.intensity = l.intensity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "ShovelAttack(Clone)")        // Gross - use more versatile approach to detect shovel
        {
            Destroy(gameObject);
        }
    }
}
