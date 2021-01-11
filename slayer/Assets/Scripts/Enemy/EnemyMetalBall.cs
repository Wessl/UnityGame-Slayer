using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class EnemyMetalBall : MonoBehaviour
{
    // References to gameobjects being used in the code
    public Rigidbody2D bd;
    public new BoxCollider2D collider;
    public Sprite spriteToSwapToWhenHit;
    [Tooltip("The particle system to use when this enemy dies")]
    public GameObject deathParticleSystem;

    private GameObject sfx;        // reference to the prefab we will get audio info from

    // Private global vars
    private bool _waiting = false;
    private bool _beginning = true;
    
    // Various parameters used in the code, can be changed via editor
    [SerializeField] private float spd = 1f;
    [SerializeField] private int livesLeft = 2;
    [Tooltip("How much this enemy will fly away from the player when hit")]
    [SerializeField] private float punchStrength = 200f;
    [SerializeField] private float waitTime = 0.1f;
    [SerializeField] private float spinSpeed = 1000f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _waiting = true;     // set to waiting while setting things up
        sfx = GameObject.FindWithTag("SFXPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (_beginning)
        {
            CreateEnemy();
        }

        if (!_waiting)
        {
            StartCoroutine(MoveRandom());
        }
        // Rotate me around - depending on how visible I am ( spin is slow while spawning, maxes out when "spawn" is complete
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        transform.Rotate(c.a * spinSpeed * Time.deltaTime * Vector3.forward);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack"))
        {
            livesLeft--;
            var pos = transform.position;
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            if (livesLeft <= 0)
            {
                // Instantiate death effect particle system
                Instantiate(deathParticleSystem, pos, Quaternion.identity);
                sfx1.MetalBallHitTwo();
                Destroy(gameObject);
            }
            else
            {
                // We have been hit, time to swap to "bloodied" sprite and shoot off in the opposite direction of the hit (assuming two lives on this guy)
                var vectorFromPlayer = pos - other.transform.position;
                bd.AddForce(vectorFromPlayer.normalized * punchStrength);
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = spriteToSwapToWhenHit;
                sfx1.MetalBallHitOne();
                // Make the movement more dramatic - enemy is now angery >:( how dare you hit me
                spinSpeed *= 1.5f;
                spd *= 1.5f;
            }
            
        }
    }

    private IEnumerator MoveRandom()
    {
        _waiting = true;
        var xdir = Random.Range(-1f * spd, spd);
        var ydir = Random.Range(-1f * spd, spd);
        Vector2 v = new Vector2(xdir, ydir);
        bd.AddForce(v * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(waitTime);
        _waiting = false;
    }

    void CreateEnemy()
    {
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a += + 0.3f * Time.deltaTime;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
        if (c.a > 0.95f)
        {
            _beginning = false;  // no more beginning is to say "we're done initializing"
            bd.WakeUp();         // enable collisions once it is visible
            collider.enabled = true;
            _waiting = false;    // waiting is to just turn on the behaviour that handles movement
        }
    }
}