using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoneAlisa : MonoBehaviour
{
    public Transform playerTransform;
    [Tooltip("This should be the rigidbody of the skeleton BODY")]
    public Rigidbody2D bd;
    public float spd;
    public float waitTime;
    private bool _waiting = false;
    public int livesLeft;
    private bool isDead = false;

    public float maxVelocity;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!_waiting)
        {
            StartCoroutine(MoveTowardsPlayer());
        }
        if(bd.velocity.sqrMagnitude > maxVelocity)
        {
            //smoothness of the slowdown is controlled by the 0.99f, 
            //0.5f is less smooth, 0.9999f is more smooth
            bd.velocity *= 0.9f;
        }

        if (isDead)
        {
            Death();
        }
    }
    private IEnumerator MoveTowardsPlayer()
    {
        _waiting = true;
        var v = new Vector3(0, 0, 0);
        if (playerTransform != null)
        {
            v = playerTransform.position - bd.transform.position;
        }

        // Randomize how strongly we are moving towards the player
        bd.AddForce(spd * Time.deltaTime * v, ForceMode2D.Impulse);
        yield return new WaitForSeconds(waitTime);
        _waiting = false;
    }

    public void DecreaseLives()
    {
        livesLeft--;
        if (livesLeft <= 0)
        {
            bd.isKinematic = false;
            isDead = true;
            var bds = GetComponentsInChildren<Rigidbody2D>();
            var colliders = GetComponentsInChildren<Collider2D>();
            foreach (var body in bds)
            {
                body.isKinematic = false;
            }
            foreach (var col in colliders)
            {
                col.enabled = false;
            }
        }   
    }

    private void Death()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spr in sprites)
        {
            var c = spr.color;
            c.a -= 0.3f * Time.deltaTime;
            spr.GetComponent<SpriteRenderer>().color = c;
            if (c.a <= 0.02f)
            {
                var spawners = GameObject.FindGameObjectsWithTag("Spawner");
                foreach (var spawner in spawners)
                {
                    Destroy(spawner);
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("BoundTrigger"))  
        {
            var d = new Vector3(bd.velocity.x, bd.velocity.y, 0);
            var v = other.transform.position;
            var n = v.normalized;
            var r = d - 2 * Vector3.Dot(n, d) * n;
            bd.velocity = r;
        }
    }

    
    
}
