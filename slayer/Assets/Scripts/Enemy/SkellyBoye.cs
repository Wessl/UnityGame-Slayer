using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellyBoye : MonoBehaviour
{
    [SerializeField] private float speed;
    [Tooltip("The particle system to use when a skellyboye dies")]
    public GameObject skellyBoyeParticleSystem;
    private GameObject sfx;        // reference to the prefab we will get audio info from

    public Rigidbody2D bd;

    void Awake()
    {
        gameObject.tag = "Untagged";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        sfx = GameObject.FindWithTag("SFXPlayer");
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        // if way out of bounds, just remove it
        if (transform.position.magnitude > 3)
        {
            Destroy(gameObject);
        }
    }
    // Starts Skeletons movement. Will move to the left if spawning on the right, and right if spawn on left
    private void Move()
    {
        var xdir = 0f;
        if (transform.position.x > 0)
        {
            xdir = speed * -1f;
        }
        else
        {
            xdir = speed * 1f;
            var ls = transform.localScale;
            transform.localScale = new Vector3(ls.x * -1, ls.y, ls.z);
        }
        Vector2 v = new Vector2(xdir, 0);
        bd.AddForce(v, ForceMode2D.Impulse);
        gameObject.tag = "Enemy";
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        // If the collision is from a player attack, die instantly (no lives on this object as of now)
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack") || other.transform.CompareTag("Lightning"))
        {
            var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
            sfx1.WoodHit();
            Instantiate(skellyBoyeParticleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
        }
    }
}
