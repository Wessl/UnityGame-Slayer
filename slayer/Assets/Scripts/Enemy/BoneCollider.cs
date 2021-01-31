using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCollider : MonoBehaviour
{
    private BoneAlisa _boneAlisaRef;
    public float punchStrength;
    private GameObject _sfx;        // reference to the prefab we will get audio info from
    public Rigidbody2D myBD;
    public GameObject boneParticleSystem;

    private void Start()
    {
        _boneAlisaRef = GetComponentInParent<BoneAlisa>();
        _sfx = GameObject.FindWithTag("SFXPlayer");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("SwordAttack") || other.transform.CompareTag("LongSwordAttack"))
        {
            _boneAlisaRef.DecreaseLives();
            var pos = transform.position;
            var sfx1 = _sfx.GetComponent<SFXControllerEnemy>();
            var vectorFromPlayer = pos - other.transform.position;
            myBD.AddForce(vectorFromPlayer.normalized * punchStrength);
            Instantiate(boneParticleSystem, transform.position, Quaternion.identity);
            sfx1.WoodHit();
        }
    }
}
