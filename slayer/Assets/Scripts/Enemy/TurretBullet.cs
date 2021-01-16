using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    // Reference to rigidbody
    Rigidbody2D bd;
    // the speed of which this object should move at
    [SerializeField] private float speed = 0.015f;
    [SerializeField] private float deflectionSpeedMultiplier = 3f;
    private bool _hasBeenReflectedAlready = false;
    private Vector3 _playerPos;
    void Start()
    {
        bd = GetComponent<Rigidbody2D>();
        bd.velocity = transform.up * speed;
        try
        {
            _playerPos = GameObject.FindWithTag("Player").transform.position;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {
        // If this object is too far away from the center field, remove it from the game to not waste performance
        transform.Rotate(1000f * Time.deltaTime * Vector3.forward);
        
        if (transform.position.magnitude > 3)
        {
            Destroy(gameObject);
        }
    }

    // If I get hit by a player weapon that can deflect bullets, make me move away directly from player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LongSwordAttack"))
        {
            // Oh, I'm die - thank you forever.
            bool reflects = false;
            try
            {
                reflects = other.GetComponent<SwingSword>().BulletReflector();
            }
            catch (Exception e)
            {
                Debug.Log(e + ", was caught in TurretBullet in OnTriggerEnter2D");
            }
            if (reflects)
            {
                var d = transform.position;
                var v = _playerPos;     // Normally we would want the point of collision, but now we just want to fuck off directly away from the player
                var n = v.normalized;
                var r = d - n;
                bd.velocity = r * deflectionSpeedMultiplier;
                _hasBeenReflectedAlready = true;     // Can only be deflected once by weapons - maybe dumb, for now it's a quick fix that works
            }
        }
        else if (other.CompareTag("BoundTrigger"))
        {
            // Oh, I am collide with bound. Thank you forever. (bullet collides with some kind of boundary, delete)
            Destroy(gameObject);
            
        }
    }
}
