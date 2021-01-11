using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    // Reference to rigidbody
    Rigidbody2D bd;
    // the speed of which this object should move at
    [SerializeField] private float speed = 0.015f;
    void Start()
    {
        bd = GetComponent<Rigidbody2D>();
        bd.velocity = transform.up * speed;
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
}
