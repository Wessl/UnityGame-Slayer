﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTurret : MonoBehaviour
{
    // References to other components on this gameobject - assigned via editor
    public Rigidbody2D bd;
    public new CircleCollider2D collider;
    public GameObject turretBullet;                         // The bullet prefab to spawn
    // Some IEnumerator bools to halt certain actions
    private bool waiting = false;
    private bool beginning = true;
    private bool waitingFire = false;
    private GameObject sfx;        // reference to the prefab we will get audio info from
    public GameObject turretDeathParticleSys;

    [SerializeField] private float fireSpeed = 1f;
    public GameObject spawnCompletionPS;

    private void Awake()
    {
        gameObject.tag = "Untagged";
    }


    // Start is called before the first frame update
    void Start()
    {
        waiting = true;     // set to waiting while setting things up
        sfx = GameObject.FindWithTag("SFXPlayer");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (beginning)
        {
            CreateEnemy();
        }
        // If it's not the very beginning (not yet fully spawned), or we're not waiting on a Fire cooldown, or waiting (why?), then Fire bullets. 
        if (!waitingFire && !beginning && !waiting)
        {
            StartCoroutine(Fire());
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // If the collision is from a sword, die instantly (no lives on this object as of now)
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack") || other.transform.CompareTag("Lightning"))
        {
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            sfx1.StoneHit();
            Instantiate(turretDeathParticleSys, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // If this gameobject hits a Boundary Trigger, the direction should be reversed in order to "bounce" back
        else if (other.transform.CompareTag("BoundTrigger"))    
        {
            waiting = false;
            var d = new Vector3(bd.velocity.x, bd.velocity.y, 0);
            var v = other.transform.position;
            var n = v.normalized;
            var r = d - 2 * Vector3.Dot(n, d) * n;
            bd.velocity = r;
        }
    }

    // Begins the movement logic, which is a random direction and speed in 2 dimensions
    private void Move()
    {
        waiting = true;
        var xdir = Random.Range(-0.3f, 0.3f);
        var ydir = Random.Range(-0.3f, 0.3f);
        Vector2 v = new Vector2(xdir, ydir);
        bd.AddForce(v, ForceMode2D.Impulse);
    }

    // Spawn this enemy by slowly increasing color alpha, enabling collision once a threshold is reached
    void CreateEnemy()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = c.a + Time.deltaTime * 1f;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        if (c.a > 0.98f)
        {
            beginning = false;  // beginning is to say "we're done initializing"
            bd.WakeUp();        // enable collisions once it is visible
            collider.enabled = true;
            waiting = false;    // waiting is to just turn on the behaviour that handles movement
            gameObject.tag = "Enemy";
            var spawnPS = Instantiate(spawnCompletionPS, transform.position, Quaternion.identity);
            spawnPS.transform.localScale *= 2;
            Move();
        }
    }

    // Call this to spawn 8 bullets in different directions out from the center of the object
    private IEnumerator Fire()
    {
        waitingFire = true;
        yield return new WaitForSeconds(fireSpeed);
        // spawn 8 bullets in different directions, evenly covering all directions of a circle (360/8)=45
        var rot = Quaternion.Euler(Vector3.forward);
        for (int i = 0; i < 8; i++)
        {
            rot = rot * Quaternion.Euler(Vector3.forward * 45);         // Iterate rotation forward, Quaternions achieve this via multiplication rather than addition
            Instantiate(turretBullet, transform.position, rot);
            
        }
        waitingFire = false;
    }
}