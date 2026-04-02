using UnityEngine;
using System.Collections;

public class CollisionDetect : MonoBehaviour
{

    [SerializeField] GameObject groundPlane;

    private Rigidbody rb;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == groundPlane)
        {
            Vector3 vel = rb.linearVelocity;
            if (vel.magnitude > 1.0f)
            {
                Debug.LogError("Crash");
                Application.Quit();
                Debug.Break();
            }
            else
            {
                Debug.Log("Hit ground plane");
            }
        }
        else
        {
            Debug.LogError("Crash");
            Application.Quit();
            Debug.Break();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
