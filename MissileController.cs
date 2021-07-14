using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    private float velocity = 10.0f;
    private Vector3 moveDirection;
    public Vector3 enemyPos;
    private Rigidbody missileRb;
    public float impactForce;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 ImpactDirection = (transform.position - collision.gameObject.transform.position).normalized;
            enemyRb.AddForce(ImpactDirection * impactForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
