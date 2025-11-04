using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Animator animator; 
    [SerializeField] private SpeedDisplay speedDisplay; 

    [Header("Speed Limits")]
    [SerializeField] private float minSpeed = 0f; 
    [SerializeField] private float maxSpeed = 14f; 
    [SerializeField] private float cappedSpeed = 14f; //max speed

    [Header("Animation Speed Control")]
    [SerializeField] private float speedMultiplier = 0.1f; 

    void Update()
    {
       
        float speed = speedDisplay.Speed;

       
        if (speed > cappedSpeed)
        {
            speed = cappedSpeed;
        }

       
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        
        animator.SetFloat("Speed", speed * speedMultiplier);

       
        if (speed > 0f)
        {
           
            animator.SetBool("IsWalking", true);
        }
        else
        {
          
            animator.SetBool("IsWalking", false);
        }
    }
}
