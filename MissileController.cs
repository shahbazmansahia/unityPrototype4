using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    private float velocity = 10.0f;
    private Vector3 moveDirection;
    public Vector3 enemyPos;
    private Rigidbody missileRb;
    public float impactForce = 500.0f;
    // Start is called before the first frame update
    void Start()
    {
        missileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = (enemyPos - transform.position).normalized;
        missileRb.AddForce(moveDirection * velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && (enemyPos != null))
        {
            
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 ImpactDirection = (other.gameObject.transform.position - transform.position).normalized;
            enemyRb.AddForce(ImpactDirection * impactForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
