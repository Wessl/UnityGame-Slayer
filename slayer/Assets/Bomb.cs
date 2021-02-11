using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float fuseTime;
    public GameObject explosionPrefab;
    public GameObject fuse;
    private GameObject sfx;
    // Start is called before the first frame update
    private void Start()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        Instantiate(fuse, transform.position + 0.1f * transform.up, Quaternion.identity);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().AlterAttackSpawnDistance(0.2f);
        sfx = GameObject.FindWithTag("SFXPlayer");
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(fuseTime);
        // Trigger explosion
        Instantiate(explosionPrefab, transform.position, quaternion.identity);
        var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
        sfx1.Boom();
        Destroy(gameObject);
    }


}
