using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // some changeable sword specific parameters (normal, shortsword, demonic shortsword, etc...)
    [SerializeField] private float sizeMultiplier = 1;

    [SerializeField] private int moveDirection = 1;

    [SerializeField] private float pullBackSpeed = -1f;
    // Should be changed to a float value if the pull back distance needs to be changed
    [SerializeField] private float changePullBackDistance = 1f;
    // Following is only for really long weapons
    public bool shouldBePulledBackFurther = false;

    private PlayerController player;
    // slowly pull back and make invisible, then delete. (alternatively do a shrinking along the length?)
    void Start()
    {
        transform.localScale *= sizeMultiplier;         // change size depending on which weapon it is - collider and all
        // This entire solution is so bad
        if (shouldBePulledBackFurther)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().AlterAttackSpawnDistance(changePullBackDistance);
        }
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
}
