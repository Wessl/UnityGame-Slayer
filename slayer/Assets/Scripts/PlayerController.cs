using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 5f;       // speed variable, can be changed in editor
    public Rigidbody2D bd;                          // The rigidbody of the player
    private GameObject _attackObject;               
    public GameObject deathPanel;
    private bool _attackOptionTowardsMouse = false;
    private GameManager _gm;
    private Vector3 myLocation;
    private int swordSwingFlipper = 1;
    [Tooltip("0.24 is pretty good for a default value for swords")]
    [SerializeField] private float attackSpawnDistance = 0.24f;

    void Start()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _attackObject = _gm.GetAllWeapons()[PlayerPrefs.GetInt("ChosenWeapon")];
        if (PlayerPrefs.GetInt("SwingType") == 1)
        {
            _attackOptionTowardsMouse = true;
        }
    }
    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        var bdVelocityRef = bd.velocity;
        // Position
        var vt = Input.GetAxisRaw("Vertical");
        var hz = Input.GetAxisRaw("Horizontal");
        bd.velocity = new Vector2(hz * speed, vt * speed) * Time.fixedDeltaTime;
        // Rotation
        float angle = Mathf.Atan2(bdVelocityRef.y, bdVelocityRef.x) * Mathf.Rad2Deg;
        if (_attackOptionTowardsMouse)
        {
            myLocation = transform.position;
            var mousePosition = Input.mousePosition;
            Vector3 targetLocation = Camera.main.ScreenToWorldPoint(mousePosition);
            targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position
            // vector from this object towards the target location
            Vector3 vectorToTarget = targetLocation - myLocation;
            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: vectorToTarget);
            transform.rotation = targetRotation;
        }
        else if (!_attackOptionTowardsMouse && bdVelocityRef != Vector2.zero)
        {
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
        
    }

    void Attack()
    {
        // An attack is triggered if the player hits either Space or clicks their mouse/ taps their screen - could be nicer/ cleaner / more versatile, works for now 
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            // There are two options of attacking, one is in the direction of the player's mouse pointer
            if (_attackOptionTowardsMouse)
            {
                myLocation = transform.position;
                var mousePosition = Input.mousePosition;
                Vector3 targetLocation = Camera.main.ScreenToWorldPoint(mousePosition);
                targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position
     
                // vector from this object towards the target location
                Vector3 vectorToTarget = targetLocation - myLocation;

                // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: vectorToTarget);
                
                if (_attackObject.CompareTag("SwordAttack") || _attackObject.CompareTag("LongSwordAttack"))
                {
                    myLocation += vectorToTarget.normalized * attackSpawnDistance;
                }
                var obj = Instantiate(_attackObject, myLocation, targetRotation);
                if (_attackObject.CompareTag("LongSwordAttack"))
                {
                    HandleLongSwordAttack(transform.position, obj);
                }
                
            }
            // And the other is "forward", which is the direction that is = to transform.forward of the gameobject
            else
            {
                // First, spawn attacking object in front of player (location and movement depends on weapon type)
                var currentTransform = gameObject.transform;
                var currentPosition = currentTransform.position;
                myLocation = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
                // If the attack object is any of these two, spawn them slightly in front of the player - 0.24f can be changed, should maybe be a variable
                if (_attackObject.CompareTag("SwordAttack") || _attackObject.CompareTag("LongSwordAttack"))
                {
                    myLocation += transform.up * attackSpawnDistance;
                }
                var obj = Instantiate(_attackObject, myLocation, currentTransform.rotation);
                // Special logic needed for "long swords", above "obj" reference is needed for some info
                if (_attackObject.CompareTag("LongSwordAttack"))
                {
                    HandleLongSwordAttack(transform.position, obj);
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If this is the case, the player should die
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyRemains") || other.CompareTag("Projectile"))
        {
            // Show the death screen first, then destroy player gameObject
            deathPanel.SetActive(true);
            string timePassedString = GameObject.FindWithTag("TimerText").GetComponent<Timer>().GetTimePassedAsString();
            deathPanel.GetComponentInChildren<Text>().text = String.Format("You died, after surviving for {0}", timePassedString);
            Destroy(gameObject);
        }
    }

    // Technically rotating sword attack - What it does is:
    // 1. Set position of rotation of the sword (so it rotates around the right point and not around its own center)
    // 2. Flip the swordSwingFlipper variable - causes the direction of the swing to rotate back and forth between "left" and "right" relative to the swing direction vector (purely for taste reasons)
    void HandleLongSwordAttack(Vector3 pos, GameObject obj)
    {
        var longsword = obj.GetComponent<SwingSword>();
        longsword.SetRotLoc(pos);
        swordSwingFlipper *= -1;
        longsword.FlipAxis(swordSwingFlipper);
    }

    public void AlterAttackSpawnDistance(float c)
    {
        attackSpawnDistance = c;
    }
}
