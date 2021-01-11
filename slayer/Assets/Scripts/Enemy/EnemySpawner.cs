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
        

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        var randomizedDelay = UnityEngine.Random.Range(0, randomizedDelayFactor);
        yield return new WaitForSeconds(spawnTimeDelay + randomizedDelay);
        waiting = false;
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
