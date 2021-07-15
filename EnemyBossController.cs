using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    private float velocity = 1.0f;
    private Rigidbody enemyRb;
    private GameObject sahil;

    public float spawnInterval = 5.0f;
    private float nextSpawn;

    public int minionSpawnCount;

    private SpawnManager spawnManger;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        sahil = GameObject.Find("Sahil");

        spawnManger = FindObjectOfType<SpawnManager>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (sahil.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * velocity);

        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnInterval;
            spawnManger.SpawnMinions(minionSpawnCount);
        }

        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }

        
    }
}
