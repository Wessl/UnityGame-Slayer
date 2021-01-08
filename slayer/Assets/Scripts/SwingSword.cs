using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSword : MonoBehaviour
{
    // Some changeable sword specific parameters (normal, shortsword, demonic shortsword, etc...)
    [SerializeField] private float sizeMultiplier = 1;
    [SerializeField] private int rotationSpeed = 500;
    // Default point and axis, used for rotating the object
    private Vector3 point = new Vector3(0,0,0);
    private Vector3 axis = new Vector3(0,0,1);
    // Sprite reference
    private SpriteRenderer mySpr;

    void Awake()
    {
        mySpr = gameObject.GetComponent<SpriteRenderer>();
    }

    // pull side to side and make invisible, then delete. (alternatively do a shrinking along the length?)
    void Start()
    {
        transform.localScale *= sizeMultiplier;         // change size depending on which weapon it is - collider and all
    }
    void Update()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = c.a * 0.97f;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        
        // Swing/Rotate sideways
        transform.RotateAround(point, axis, Time.deltaTime * rotationSpeed);
        if (c.a < 0.1f) {
            Destroy(gameObject);
        }
    }

    // Sets the "point" which is where we rotate around
    public void SetRotLoc(Vector3 myRotloc)
    {
        point = myRotloc;
    }

    // Magic axis action stuff
    public void FlipAxis(int a)
    {
        axis *= a;
        if (a > 0)
        {
            mySpr.flipX = false;
        }
        else
        {
            mySpr.flipX = true;
        }
        
    }
}
