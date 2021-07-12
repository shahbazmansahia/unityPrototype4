using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float velocity = 1.0f;
    private Rigidbody enemyRb;
    private GameObject sahil;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        sahil = GameObject.Find("Sahil");

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (sahil.transform.position - transform.position).normalized;
        
        enemyRb.AddForce( lookDirection * velocity);
    }
}
