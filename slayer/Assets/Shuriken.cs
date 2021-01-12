using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private float flySpeed;
    [SerializeField] private float spinSpeed = 1000f;
    private Vector3 dir;    // The direction this is thrown
    public Rigidbody2D bd;
    void Start()
    {
        dir = transform.up;
        bd.velocity = flySpeed * dir;
    }
    void Update()
    {
        // Move along
        transform.Rotate(spinSpeed * Time.deltaTime * Vector3.forward);
        if (transform.position.magnitude > 3f)
        {
            Destroy(gameObject);
        }
    }
}