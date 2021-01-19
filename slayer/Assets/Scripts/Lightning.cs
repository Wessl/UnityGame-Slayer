using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update
    public void Remove()
    {
        Destroy(gameObject);
    }
}
