using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SahilController : MonoBehaviour
{
    public float velocity = 5.0f;
    private GameObject focalPoint;
    private Rigidbody sahilRb;
    // Start is called before the first frame update
    void Start()
    {
        sahilRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        sahilRb.AddForce(focalPoint.transform.forward * velocity * forwardInput);
    }
}
