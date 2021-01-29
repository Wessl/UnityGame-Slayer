using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Various internal variables
    private int _enemiesAlive;
    private int _spawnersAlive;
    private bool _waiting = false;
    private bool _won     = false;
    private GameObject _musicPlayer;
    
    // GameObjects assigned via editor
    public GameObject winPanel;             // The panel that shows up once a level has been won
    public GameObject newWeaponPanel;       // The panel that shows ONLY the first time a level has been cleared
    public GameObject[] allAttackObjects;   // Contains all attack objects available to pick from
    
    void Start()
    {
        _spawnersAlive = GameObject.FindGameObjectsWithTag("Spawner").Length;
        _enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _musicPlayer = GameObject.FindWithTag("MusicPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_waiting)
        {
            StartCoroutine(CheckAlive());
        }
        // if there are no spawners left alive, there won't be any more enemies spawning, so check how many enemies are left
        // - this needs to happen constantly in order to trigger instant feedback once the last enemy is slain
        if (_spawnersAlive == 0)
        {
            _enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        
        // When no enemies or spawners are left, we can safely assume current level is completed. 
        if (_enemiesAlive == 0 && _spawnersAlive == 0 && !_won)
        {
            Win();
        }
    }

    // Is only called once every scene load
    // Handles displaying win panel, unlocking potentially new weapons
    void Win()
    {
        _won = true;
        var currentLevel = SceneManager.GetActiveScene().buildIndex;                 // for now, level-1 is equivalent to buildindex 1
        if (currentLevel > PlayerPrefs.GetInt("UnlockedWeapons"))
        {
            PlayerPrefs.SetInt("UnlockedWeapons", currentLevel);                        // Unlock new weapon up to current level
            newWeaponPanel.SetActive(true);
            var attackObject = allAttackObjects[currentLevel];                          // Find the object to unlock
            Image[] children = newWeaponPanel.GetComponentsInChildren<Image>();
            foreach (Image img in children)
            {
                    if (img.gameObject.name == "NewWeaponImage")                        // What? I guess, the first object not named is the one we want... TODO: Improve this
                    {
                        var atkReference = attackObject.GetComponent<SpriteRenderer>();
                        img.sprite = atkReference.sprite;
                        img.color = atkReference.color;                                 // certain sprites look only look different due to differing color properties, inherit these for consistency
                        GameObject.Find("NewWeaponFlavorText").GetComponent<Text>().text =
                            atkReference.GetComponent<GenericAttackObject>().GETFlavorText();
                    }
            }
        }

        if (currentLevel > PlayerPrefs.GetInt("UnlockedLevels"))
        {
            PlayerPrefs.SetInt("UnlockedLevels", currentLevel);                     // Unlock new level up to current level
        }

        winPanel.SetActive(true);
        // On the Music Player, get the Music Handler component script an activate the victory song
        _musicPlayer.GetComponent<MusicHandler>().OnLevelVictory();
        
    }

    // Check how many enemies are currently alive i.e. existing in the game world
    private IEnumerator CheckAlive()
    {
        _waiting = true;
        // Wait one second - this means we only check the amount of spawners every 1 second
        yield return new WaitForSeconds(1);  
        
        _spawnersAlive = GameObject.FindGameObjectsWithTag("Spawner").Length;   // potentially expensive operation, hence why it is only performed once in a while
        _waiting = false;
    }
    
    // Used to get all the weapons stored in this scene - could this be in a prefab instead? Probably yes
    public GameObject[] GetAllWeapons()
    {
        return allAttackObjects;
    } 
}
