using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSwinger : MonoBehaviour
{
    public Vector2 swivelPoint;
    public float rotationSpeed;
    private Vector3 axis = new Vector3(0,0,1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(swivelPoint, axis, Time.deltaTime * rotationSpeed);
    }
}
