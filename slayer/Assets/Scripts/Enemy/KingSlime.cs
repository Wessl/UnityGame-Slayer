using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class KingSlime : MonoBehaviour
{
    public Rigidbody2D bd;
    public new PolygonCollider2D collider;
    [Tooltip("The particle system to use when this slime dies")]
    public GameObject slimeBallParticleSystem;

    private bool _waiting = false;
    private bool _beginning = true;
    public int _livesLeft = 10;
    [SerializeField] private float spd = 1f;
    [SerializeField] private float punchStrength = 200f;
    [SerializeField] private Transform playerTransform;
    private GameObject sfx;        // reference to the prefab we will get audio info from
    [SerializeField] private float waitTime = 0.1f;

    void Awake()
    {
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a = 0f;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
    }
    
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
            StartCoroutine(MoveTowardsPlayer());
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
        var pos = transform.position;
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack") || other.transform.CompareTag("Lightning"))
        {
            _livesLeft -= 1;
            if (_livesLeft <= 0)    // This is the killing blow
            {
                // Instantiate death effect particle system AND sfx
               sfx1.SlimeHit();
               Instantiate(slimeBallParticleSystem, pos, Quaternion.identity);
               // Make sure we remove the spawner at the same time
               var spawn = GameObject.FindWithTag("Spawner");
               Destroy(spawn);
               Destroy(gameObject); 
            }
            else // Not the killing blow
            {
                // Move me slightly
                var vectorFromPlayer = pos - other.transform.position;
                bd.AddForce(vectorFromPlayer.normalized * punchStrength);
                // Audio
                sfx1.SlimeHit();
                // Gimme some particles too
                Instantiate(slimeBallParticleSystem, transform.position, Quaternion.identity);
            }
            
        }
    }

    private IEnumerator MoveTowardsPlayer()
    {
        _waiting = true;
        var v = new Vector3(0, 0, 0);
        if (playerTransform != null)
        {
            v = playerTransform.position - this.transform.position;
        }
        // Randomize how strongly we are moving towards the player
        var fluctuator = Random.Range(0.1f, 1f) * spd;
        bd.AddForce(fluctuator * v * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(waitTime);
        _waiting = false;
    }

    void CreateEnemy()
    {
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a += + 0.2f * Time.deltaTime;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
        if (c.a > 0.97f)
        {
            _beginning = false;  // beginning is to say "we're done initializing"
            bd.WakeUp();         // enable collisions once it is visible
            collider.enabled = true;
            _waiting = false;    // waiting is to just turn on the behaviour that handles movement
        }
    }
}