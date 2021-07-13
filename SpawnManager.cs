using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    // Values to give the spawnRange a little bit of buffer to avoid objects spawning right next to the player
    //private float enemyRange = 7.0f;
    //private float sahilRange = 2.0f;
    
    private float spawnRange = 9.0f;
    private int waveNum;
    private int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        waveNum = 1;
        SpawnEnemyWave(waveNum);
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        //Debug.Log("Enemies Left: " + enemyCount);
        if (enemyCount < 1)
        {
            waveNum++;
            SpawnEnemyWave(waveNum);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        }
    }

    void SpawnEnemyWave(int value)
    {
        for (int i = 0; i < value; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
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
