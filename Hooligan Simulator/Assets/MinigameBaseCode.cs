using UnityEngine;
using TMPro;

public class MouseFollow : MonoBehaviour // minigame? do i need this 
{
    public Camera gameCamera; 
    public Transform centerObject; 
    public TextMeshProUGUI heightText; 
    public float zDistanceFromCamera = 10f; 
    public float smoothingSpeed = 5f; 

    public float divisor = 1f; 

    private Vector3 targetPosition; 
    private float radius; 

    void Start()
    {
        
        if (gameCamera == null)
        {
            gameCamera = Camera.main;
        }

        
        if (heightText == null)
        {
            Debug.LogError("TextMeshProUGUI reference is not assigned.");
            return;
        }

        
        UpdateRadiusFromText(); 
        Vector3 initialPosition = centerObject.position + Vector3.right * radius;
        transform.position = initialPosition;
        targetPosition = initialPosition;
    }

    void Update()
    {
        
        if (gameCamera != null && centerObject != null)
        {
            
            UpdateRadiusFromText();

            
            if (divisor != 0f)
            {
                radius /= divisor;
            }

           
            Vector3 mousePosition = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistanceFromCamera));
            mousePosition.z = 0f; 

            
            Vector3 direction = mousePosition - centerObject.position;

            
            float angle = Mathf.Atan2(direction.y, direction.x);

            
            Vector3 newPosition = centerObject.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            
            targetPosition = newPosition;

            
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothingSpeed * Time.deltaTime);

            
            transform.up = centerObject.position - transform.position;
        }
    }

   
    private void UpdateRadiusFromText()
    {
        if (heightText != null)
        {
            float parsedRadius;
            
            if (float.TryParse(heightText.text, out parsedRadius))
            {
                radius = parsedRadius; 
            }
        }
    }
}
