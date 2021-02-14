using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SamuraiArcher : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 playerPos;
    private Quaternion targetRotation;
    private Rigidbody2D _bd;
    public BoxCollider2D _collider;
    [Tooltip("The prefab that this enemy will fire at the player")]
    public GameObject arrowPrefab;
    private GameObject sfx;        // reference to the prefab we will get audio info from
    public GameObject deathParticleSystem;
    
    public Animator animController;
    private bool _beginning = true;
    public GameObject spawnCompletionPS;

    void Awake()
    {
        gameObject.tag = "Untagged";
    }
    
    void Start()
    {
        _bd = GetComponent<Rigidbody2D>();
        try
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;
            playerPos = _playerTransform.position;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Player is dead, thus could not find one with tag player so null: " + e);
        }
        
        sfx = GameObject.FindWithTag("SFXPlayer");
    }

    private void FixedUpdate()
    {
        if (_beginning)
        {
            CreateEnemy();
        }
        else
        {
            try
            {
                _playerTransform = GameObject.FindWithTag("Player").transform;
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Player is dead, thus could not find one with tag player so null: " + e);
            }
            if (_playerTransform != null)
            {
                targetRotation = CalculateTargetRotation();
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 9000f * Time.deltaTime);
            }
        }
    }

    // Called from archer animation asset as an animation event, on the last frame of the animation
    void FuckingFireJesusChrist()
    {
        var arrow = Instantiate(arrowPrefab, transform.position, targetRotation);
    }

    Quaternion CalculateTargetRotation()
    {
        playerPos = _playerTransform.position;
        var vectorToTarget = playerPos - transform.position;
        // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: vectorToTarget);
        return targetRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SwordAttack") || other.CompareTag("LongSwordAttack") || other.transform.CompareTag("Lightning"))
        {
            // Oh, I'm die. Thank you forever.
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            sfx1.MetalBallHitTwo();
            Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void CreateEnemy()
    {
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a += 1f * Time.deltaTime;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
        if (c.a > 0.98f)
        {
            _beginning = false;  // no more beginning is to say "we're done initializing"
            _bd.WakeUp();         // enable collisions once it is visible
            _collider.enabled = true;
            animController.SetBool("Fire", true);   // Start animating
            gameObject.tag = "Enemy";
            Instantiate(spawnCompletionPS, transform.position, Quaternion.identity);
        }
    }
}
