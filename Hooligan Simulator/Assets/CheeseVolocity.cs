using UnityEngine;

public class InitialVelocity : MonoBehaviour
{
    [SerializeField]
    private float initialSpeed = 5f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

       
        Vector3 initialVelocity = -transform.forward * initialSpeed; // Flipped direction
        rb.velocity = initialVelocity;
    }
}
