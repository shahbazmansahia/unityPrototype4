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

    //Added as a part of the alternate approach to implementing powerup/missile system
    public PowerUpType currPowerUp = PowerUpType.None;

    public GameObject missilePrefab;
    private GameObject spawnedMissile;
    private Coroutine powerUpCountdown;

    //Adding the variables below to implement the 'smash' powerup
    public float hangTime = 1.0f;
    public float waveSpeed = 5.0f;
    public float explosionForce = 20.0f;
    public float explosionRadius =  5.0f;

    bool isSmashEnabled = false;
    float floorY;

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

        // The code below has been implemented as a part of the alt. approach of homing missile systems
        if ((currPowerUp == PowerUpType.Missiles) && (Input.GetKeyDown(KeyCode.LeftControl)))
        {
            AltSpawnMissiles();
        }

        if ((currPowerUp == PowerUpType.Smash) && (Input.GetKeyDown(KeyCode.Space) && (!isSmashEnabled)))
        {
            isSmashEnabled = true;
            StartCoroutine(Smash());
            Debug.Log("SMASH!");
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            Debug.Log("Powerup Type: " + currPowerUp.ToString());
            Destroy(other.gameObject);
            //StartCoroutine(PowerupCountdownRoutine());      // Commenting it out for alternate approach
            powerupIndicator.SetActive(true);

            // Alt. approach part starts here
            if (powerUpCountdown != null)
            {
                StopCoroutine(AltPowerupCountdownRoutine());
            }
            powerUpCountdown = StartCoroutine(AltPowerupCountdownRoutine());

            // Alt. approach ends here
        }
        /** Code commented to immplement an test Alt. approach to missile spawns
         * 
        if (other.CompareTag("Powerup2"))
        {
            hasPowerup = true;
            missilesEnabled = true;
            
            Destroy(other.gameObject);
            StartCoroutine(Powerup2CountdownRoutine());
            powerupIndicator.SetActive(true);
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Enemy") && hasPowerup)       // Older implementation of if statement; alternate approach applied below this
        if (collision.gameObject.CompareTag("Enemy") && (currPowerUp == PowerUpType.Pushback))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromSahil = (collision.gameObject.transform.position - transform.position);
            // Older Debug.Log
            //Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            
            // Alt. approach Debug.Log
            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + currPowerUp.ToString());
            enemyRb.AddForce(awayFromSahil * powerupStrength, ForceMode.Impulse);
        }
    }

    /** This method replaces the functionality of the SpawnMissiles f(x) as a part of the alt. approach
     * 
     */
    void AltSpawnMissiles()
    {
        Debug.Log("AltSpawnMissiles() triggered!");
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < activeEnemies.Length; i++)
        {
            spawnedMissile = Instantiate(missilePrefab, transform.position + Vector3.up, Quaternion.identity);
            spawnedMissile.GetComponent<AltMissileController>().Fire(activeEnemies[i].transform);
            Debug.Log("Missile Spawned!");
        }
    }

    /** This segment has been rednered defunct due to the enum Powerup approach applied as a part of the alt. approach to implementing homing missiles
     * 
    // IEnumerator is basically an interface; it helps us enable the countown timer outside the update loop; think of them like a second update loop running
    IEnumerator PowerupCountdownRoutine()
    {
        // yield basically allows this f(x)/statement to run outside/independent of the update f(x)
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
        Debug.Log("Powerup deactivated");
    }
    */

    /** a function now defunct due to the alt. approach applied to homing missiles
     * 
    IEnumerator Powerup2CountdownRoutine()
    {
        // yield basically allows this f(x)/statement to run outside/independent of the update f(x)
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        missilesEnabled = false;
        powerupIndicator.SetActive(false);
        Debug.Log("Powerup2 deactivated");
    }
    */

    // Coroutine developed as a part of alt. approach to himing missiles
    IEnumerator AltPowerupCountdownRoutine()
    {
        Debug.Log("Alt. Routine Started!");
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        currPowerUp = PowerUpType.None;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator Smash()
    {
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        Debug.Log("Smash routine started!");

        // Store the Player's 'y' position so it we can return to it after the animation
        floorY = transform.position.y;

        // Calculate the time period we hover as a part of the animation
        float jumpTime = Time.time + hangTime;

        //Now move the player back down
        while(Time.time < jumpTime)
        {
            sahilRb.velocity = new Vector2(sahilRb.velocity.x, waveSpeed);
            yield return null;
        }

        for (int i = 0; i < activeEnemies.Length; i++)
        {
            // Apply an explosion force that originates from player position
            if (activeEnemies[i] != null)
            {
                activeEnemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 10.0f, ForceMode.Impulse);
            }

            // We are no longer smashing
            isSmashEnabled = false;
        }
    }
}
