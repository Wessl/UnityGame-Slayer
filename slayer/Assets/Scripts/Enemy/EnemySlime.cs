using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class EnemySlime : MonoBehaviour
{
    public Rigidbody2D bd;
    public new BoxCollider2D collider;
    public GameObject slimeRemains;
    [Tooltip("The particle system to use when this slime dies")]
    public GameObject slimeBallParticleSystem;
    private Light2D _myLight;

    private bool _waiting = false;
    private bool _beginning = true;
    [SerializeField] private float spd = 1f;
    [SerializeField] private bool useLight = false;
    [SerializeField] private float maxLightIntensity = 0.4f;
    private GameObject sfx;        // reference to the prefab we will get audio info from
    

    [SerializeField] private float waitTime = 0.1f;

    void Awake()
    {
        gameObject.tag = "Untagged";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _waiting = true;     // set to waiting while setting things up
        sfx = GameObject.FindWithTag("SFXPlayer");
        if (useLight)
        {
            _myLight = GetComponent<Light2D>();
        }
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
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack"))
        {
            if (gameObject.CompareTag("SlimeRed"))
            {
                // spawn slime remains
                var remains = Instantiate(slimeRemains, transform.position, Quaternion.identity);
                // let slime remains inherit light settings
                remains.GetComponent<SlimeRemains>().SetLightSettings(_myLight);
            }
            // Instantiate death effect particle system AND sfx
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            sfx1.SlimeHit();
            Instantiate(slimeBallParticleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveRandom()
    {
        _waiting = true;
        var xdir = Random.Range(-0.003f * spd, 0.003f * spd);
        var ydir = Random.Range(-0.003f * spd, 0.003f * spd);
        Vector2 v = new Vector2(xdir, ydir);
        bd.AddForce(v * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(waitTime);
        _waiting = false;
    }

    void CreateEnemy()
    {
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a = c.a * 1.01f;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
        if (useLight && _myLight.intensity < maxLightIntensity)
        {
            _myLight.intensity = c.a;   // Gradually increase light source intensity while spawning
        }
        if (c.a > 0.9f)
        {
            _beginning = false;  // beginning is to say "we're done initializing"
            bd.WakeUp();         // enable collisions once it is visible
            collider.enabled = true;
            _waiting = false;    // waiting is to just turn on the behaviour that handles movement
            gameObject.tag = "Enemy";
        }
    }
}