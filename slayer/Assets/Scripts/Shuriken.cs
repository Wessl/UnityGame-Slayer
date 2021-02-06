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
    public bool shouldStopFlying = false;
    public float flyDecay = 0.8f;
    private bool shouldDisappear = false;
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

    private void FixedUpdate()
    {
        if (shouldStopFlying)
        {
            bd.velocity = bd.velocity * flyDecay;
            if (bd.velocity.magnitude <= 0.3f)
            {
                Disappear();
            }
        }

        if (shouldDisappear)
        {
            Disappear();
        }
    }

    void Disappear()
    {
        Debug.Log("byue bye");
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a = c.a - Time.deltaTime * 10f;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
        // If we surpass this threshold, delete
        if (c.a <= 0.02f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            shouldDisappear = true;
        }
    }
}