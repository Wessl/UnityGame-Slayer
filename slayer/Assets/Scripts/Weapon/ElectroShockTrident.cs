using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectroShockTrident : MonoBehaviour
{
    // some changeable sword specific parameters (normal, shortsword, demonic shortsword, etc...)
    [SerializeField] private float sizeMultiplier = 1;

    [SerializeField] private int moveDirection = 1;

    [SerializeField] private float pullBackSpeed = -1f;
    // Should be changed to a float value if the pull back distance needs to be changed
    [SerializeField] private float changePullBackDistance = 1f;
    // Following is only for really long weapons
    public bool shouldBePulledBackFurther = false;
    [SerializeField] private int bouncesDesired = 2;
    public GameObject lightning;
    private GameObject sfx;        // reference to the prefab we will get audio info from
    [SerializeField] private float maxLightningRange;
    
    void Start()
    {
        transform.localScale *= sizeMultiplier;         // change size depending on which weapon it is - collider and all
        // This entire solution is so bad
        if (shouldBePulledBackFurther)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().AlterAttackSpawnDistance(changePullBackDistance);
        }
        sfx = GameObject.FindWithTag("SFXPlayer");
    }
    void Update()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = c.a - 4f * Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        
        // Move back
        transform.position +=  moveDirection * Time.deltaTime * pullBackSpeed * transform.up;
        if (c.a < 0.1f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If I collide with an enemy, cause an electroshock to the next X closest enemies, dealing damage to them as well)
        if (other.CompareTag("Enemy") || other.CompareTag("EnemySlightlyUnbound"))
        {
            var bounceTimes = bouncesDesired;
            var enemyCollidedWith = other.transform;
            var t = transform;
            var allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (allEnemies.Length <= 1)
            {   
                // since there were no subjects to bounce towards, other than the one I collided with just now, return
                return;
            }
            var myTransforms = new Transform[allEnemies.Length];
            int i = 0;
            foreach (var go in allEnemies)
            {
                myTransforms[i] = go.transform;
                i += 1;
            }

            var nClosest = myTransforms.OrderBy(z => (z.position - enemyCollidedWith.position).sqrMagnitude)
                .Take(2)   //or use .FirstOrDefault();  if you need just one
                .ToArray();
            // Now we have the object closest to us in nClosest. Let's shock them. 
            var startPos = enemyCollidedWith.position;
            // Take the one 2nd furthest away - the closest will probably be the same object!
            var endPos = nClosest[1].position;  
            Vector3 vectorToTarget = endPos - startPos;
            // Limit the range - only then does the actual lightning attack occur
            if (vectorToTarget.magnitude < maxLightningRange)
            { 
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
                var lightningAttack = Instantiate(lightning, startPos, targetRotation);
                Stretch(lightningAttack, startPos, endPos, true);
                // Sfx
                var sfx1 = sfx.GetComponent<SFXControllerEnemy>();
                sfx1.Zap();  
            }
            
        }
    }
    // Causes the lightning strike to get stretched between two points
    public void Stretch(GameObject _sprite,Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ) {
        float width = _sprite.GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.position = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.up = direction;
        if (_mirrorZ) _sprite.transform.up *= -1f;
        Vector3 scale = new Vector3(1,1,1);
        scale.y = Vector3.Distance(_initialPosition, _finalPosition) / width;
        _sprite.transform.localScale = scale;
    }
}