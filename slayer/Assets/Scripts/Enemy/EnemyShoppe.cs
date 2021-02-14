using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShoppe : MonoBehaviour
{
    public Rigidbody2D bd;
    public new BoxCollider2D collider;
    private bool waiting = false;
    private bool beginning = true;
    [SerializeField] private float spd = 1f;
    private GameObject sfx;        // reference to the prefab we will get audio info from
    public GameObject shoppeDeathSystem;
    public GameObject shoppeBoneDeathSystem;
    public GameObject spawnCompletionPS;



    [SerializeField] private float waitTime = 0.1f;

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
    void Update()
    {
        if (beginning)
        {
            CreateEnemy();
        }

        if (!waiting)
        {
            StartCoroutine(MoveRandom());
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack") || other.transform.CompareTag("Lightning"))
        {
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            sfx1.WoodHit();
            Instantiate(shoppeBoneDeathSystem, transform.position, Quaternion.identity);
            Instantiate(shoppeDeathSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveRandom()
    {
        waiting = true;
        var xdir = Random.Range(-0.003f * spd, 0.003f * spd);
        var ydir = Random.Range(-0.003f * spd, 0.003f * spd);
        Vector2 v = new Vector2(xdir, ydir);
        bd.AddForce(v * Time.deltaTime, ForceMode2D.Impulse);
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }

    void CreateEnemy()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = c.a * 1.01f;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        if (c.a > 0.9f)
        {
            beginning = false;  // beginning is to say "we're done initializing"
            bd.WakeUp();        // enable collisions once it is visible
            collider.enabled = true;
            waiting = false;    // waiting is to just turn on the behaviour that handles movement
            gameObject.tag = "Enemy";
            Instantiate(spawnCompletionPS, transform.position, Quaternion.identity);
        }
    }
}
