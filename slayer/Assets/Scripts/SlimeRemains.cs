using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SlimeRemains : MonoBehaviour
{
    // How long this object should be alive before being destroyed
    [SerializeField] public float duration;
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
}
