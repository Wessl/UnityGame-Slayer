using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    /*
     * Either, spawn all enemies outside and have them move into the space
     * Spawn by slowly coming into view via alpha changes                   (*)
     * Spawn in certain area only
     */
    private bool waiting = false;

    public GameObject enemyToSpawn;

    [SerializeField] private float spawnTimeDelay = 1f;

    [SerializeField] private float randomizedDelayFactor = 1f;

    [SerializeField] private float spawnX = 1.7f;

    [SerializeField] private float spawnY = 0.9f;

    [SerializeField] public int timeUntilActivation;
    
    [SerializeField] public int timeUntilDisabling;

    private bool spawnerActivated = false;

    [Tooltip("For levels where there are obstructions that you can't spawn inside")]
    public bool checkSpawnLocations = false;
    public GameObject spawnLocationChecker;
    public Collider2D[] illegalColliderSpawns;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!spawnerActivated)
        {
            StartCoroutine(PauseFor());
        }

        StartCoroutine(DisableWaiter());
    }

    // Update is called once per frame
    void Update()
    {
        if (!waiting && spawnerActivated)
        {
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        waiting = true;
        var currentPos = transform.position;
        var spawnPosition = new Vector2(currentPos.x, currentPos.y);
        spawnPosition.x += UnityEngine.Random.Range(-1f * spawnX, spawnX);
        spawnPosition.y += UnityEngine.Random.Range(-1f * spawnY, spawnY);

        // Check if the randomized location overlaps with any illegal spawning areas
        if (checkSpawnLocations)
        {
            if (!IsValidSpawnLocation(spawnPosition))
            {
                StartCoroutine(Spawn());
            }
            else
            {
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                var randomizedDelay = UnityEngine.Random.Range(0, randomizedDelayFactor);
                yield return new WaitForSeconds(spawnTimeDelay + randomizedDelay);
                waiting = false;
            }
        }
    }

    private bool IsValidSpawnLocation(Vector2 spawnPosition)
    {
        // If this reference point is on top of a "Default" layer, we are probably touching something we shouldn't, so:
        var overlappingBox = Physics2D.BoxCast(spawnPosition, new Vector2(0.1f, 0.1f), 0f, Vector2.up);
        Physics2D.SyncTransforms();
        foreach (var illegalCollider in illegalColliderSpawns)
       {
           if (overlappingBox.collider == illegalCollider)
           {
               // Move over by X units to some random point on a circle scaled down slightly
               var rand = UnityEngine.Random.insideUnitCircle.normalized * 0.5f;
               Debug.Log("Spawn location invalid, trying again");
               return false;
           }
       }
        return true;
    }


    private IEnumerator PauseFor()
    {
        yield return new WaitForSeconds(timeUntilActivation);
        spawnerActivated = true;
    }

    private IEnumerator DisableWaiter()
    {
        yield return new WaitForSeconds(timeUntilDisabling);
        Destroy(gameObject);
    }
    
}
