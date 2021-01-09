using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class SlimeRemains : MonoBehaviour
{
    // How long this object should be alive before being destroyed
    [SerializeField] public float duration;
    public Light2D _myLight;
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
        _myLight = l;
    }
}
