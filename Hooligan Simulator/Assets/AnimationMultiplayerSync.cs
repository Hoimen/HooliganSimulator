using UnityEngine;
using TMPro;

public class SpeedDisplay : MonoBehaviour
{
    public TextMeshProUGUI speedText;

    private Vector3 previousPosition;
    private float speed;
    private float timeElapsed = 0f;
    public float updateInterval = 0.5f; 

    public float Speed => speed; 

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
       
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= updateInterval)
        {
            
            Vector3 horizontalMovement = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(previousPosition.x, 0, previousPosition.z);

            // Calculate speed (distance/time)
            speed = horizontalMovement.magnitude / timeElapsed;

            
            previousPosition = transform.position;

            
            timeElapsed = 0f;

            
            speedText.text = "Speed: " + speed.ToString("F2");
        }
    }
}
