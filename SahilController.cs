using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SahilController : MonoBehaviour
{
    public float velocity = 5.0f;
    private GameObject focalPoint;
    private Rigidbody sahilRb;
    public bool hasPowerup = false;
    public bool missilesEnabled = false;
    public bool isGameOver = false;
    private float powerupStrength = 5.0f;
    public GameObject powerupIndicator;


    // Start is called before the first frame update
    void Start()
    {
        sahilRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        powerupIndicator.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        sahilRb.AddForce(focalPoint.transform.forward * velocity * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.SetActive(true);
        }

        if (other.CompareTag("Powerup2"))
        {
            hasPowerup = true;
            missilesEnabled = true;
            Destroy(other.gameObject);
            StartCoroutine(Powerup2CountdownRoutine());
            powerupIndicator.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromSahil = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);

            enemyRb.AddForce(awayFromSahil * powerupStrength, ForceMode.Impulse);
        }
    }

    // IEnumerator is basically an interface; it helps us enable the countown timer outside the update loop; think of them like a second update loop running
    IEnumerator PowerupCountdownRoutine()
    {
        // yield basically allows this f(x)/statement to run outside/independent of the update f(x)
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
        Debug.Log("Powerup deactivated");
    }

    IEnumerator Powerup2CountdownRoutine()
    {
        // yield basically allows this f(x)/statement to run outside/independent of the update f(x)
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        missilesEnabled = false;
        powerupIndicator.SetActive(false);
        Debug.Log("Powerup2 deactivated");
    }
}
