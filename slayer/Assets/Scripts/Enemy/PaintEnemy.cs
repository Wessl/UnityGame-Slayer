using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PaintEnemy : MonoBehaviour
{
    // References to other components and values on this gameobject - assigned via editor
    public Rigidbody2D bd;
    public new PolygonCollider2D collider;
    public float moveSpeed = 0.6f;
    public Sprite[] splashImages;
    public GameObject paintSplashHolderObject;
    public GameObject woodSplinterPS;
    // Some IEnumerator bools to halt certain actions
    private bool _waiting = false;
    private bool _beginning = true;
    private GameObject sfx;
    private Animator anim;
    
    public GameObject spawnCompletionPS;

    private void Awake()
    {
        gameObject.tag = "Untagged";
        // Set start color to almost invisible, which allows CreateEnemy() to "rev up" the creation of this guy
        GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.01f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _waiting = true;     // set to waiting while setting things up
        sfx = GameObject.FindWithTag("SFXPlayer");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_beginning)
        {
            CreateEnemy();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // If this gameobject hits a Boundary Trigger, the direction should be reversed in order to "bounce" back
        if (other.transform.CompareTag("BoundTrigger"))    
        {
            _waiting = false;
            var d = new Vector3(bd.velocity.x, bd.velocity.y, 0);
            var v = other.transform.position;
            var n = v.normalized;
            var r = d - 2 * Vector3.Dot(n, d) * n;
            bd.velocity = r;
        }
        // If the collision is from a sword, die instantly (no lives on this object as of now)
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack") || other.transform.CompareTag("Lightning"))
        {
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            sfx1.StoneHit();
            Instantiate(woodSplinterPS, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Begins the movement logic, which is a random direction and speed in 2 dimensions
    private void Move()
    {
        _waiting = true;
        var xdir = Random.Range(-moveSpeed, moveSpeed);
        var ydir = Random.Range(-moveSpeed, moveSpeed);
        Vector2 v = new Vector2(xdir, ydir);
        bd.AddForce(v, ForceMode2D.Impulse);
        Debug.Log(bd.velocity);
    }

    // Spawn this enemy by slowly increasing color alpha, enabling collision once a threshold is reached
    void CreateEnemy()
    {
        Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        c.a = c.a + Time.deltaTime * 0.5f;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
        // If we surpass this threshold, activate everything - even Move() is called from here
        if (c.a > 0.98f)
        {
            _beginning = false;  // beginning is to say "we're done initializing"
            bd.WakeUp();         // enable collisions once it is visible
            collider.enabled = true;
            _waiting = false;    // waiting is to just turn on the behaviour that handles movement
            gameObject.tag = "Enemy";
            anim.SetTrigger("BeginAnimating");
            Instantiate(spawnCompletionPS, transform.position, Quaternion.identity);
            Move();
        }
    }

    // Make a splash of color on the ground on the landing location. Called from animation event. 
    public void SplashColor()
    {
        // Pick random splash image
        Sprite img = splashImages[Random.Range(0, splashImages.Length)];
        // Create splash on the ground directly underneath paintbrush (with some padding since the brush is somewhat below the center) with random rotation
        var splash = Instantiate(paintSplashHolderObject, transform.position + 0.135f * Vector3.down, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        splash.GetComponent<SpriteRenderer>().sprite = img;
        // Set Random Color
        splash.GetComponent<SpriteRenderer>().color =
            new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
   
    }
    
}
