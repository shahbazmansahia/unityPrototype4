using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public GameObject missile;
    private GameObject[] activeEnemies;

    // Values to give the spawnRange a little bit of buffer to avoid objects spawning right next to the player
    //private float enemyRange = 7.0f;
    //private float sahilRange = 2.0f;
    
    private float spawnRange = 9.0f;
    private int waveNum;
    private int enemyCount;

    private float missileStrength;

    private GameObject sahilGameObj;
    private SahilController sahilControllerScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        sahilGameObj = GameObject.Find("Sahil");
        sahilControllerScript = sahilGameObj.GetComponent<SahilController>();
        waveNum = 1;
        SpawnEnemyWave(waveNum);
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = activeEnemies.Length;
        //Debug.Log("Enemies Left: " + enemyCount);
        if (enemyCount < 1)
        {
            waveNum++;
            SpawnEnemyWave(waveNum);
            SpawnPowerup();
        }
        // TO DO: ADD A TIMER CONDITION TO AVOID MISSILE SPAM
        if (sahilControllerScript.missilesEnabled && (!sahilControllerScript.isGameOver) && (Input.GetKeyDown(KeyCode.LeftControl)))
        {
            SpawnMissiles();
        }
    }

    void SpawnPowerup()
    {
        int powerup2Spawn = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[powerup2Spawn], GenerateSpawnPosition(), powerupPrefabs[powerup2Spawn].transform.rotation);
    }

    void SpawnEnemyWave(int value)
    {
        
        for (int i = 0; i < value; i++)
        {
            int enemyType = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyType], GenerateSpawnPosition(), enemyPrefabs[enemyType].transform.rotation);
        }
    }

    void SpawnMissiles()
    {
        MissileController missileControllerScript;
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyCount != 0)
            {
                missileControllerScript = missile.GetComponent<MissileController>();
                missileControllerScript.enemyPos = activeEnemies[i].gameObject.transform.position;
                Instantiate(missile, (sahilGameObj.transform.position + sahilGameObj.transform.localScale), sahilGameObj.transform.rotation);

            }

        }
    }
    public Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}
