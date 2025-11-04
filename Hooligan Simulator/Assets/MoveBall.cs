using UnityEngine;
using TMPro;

public class AutoMoveToCenter2D : MonoBehaviour
{
    public Transform centerObject; 
    public TextMeshProUGUI moveSpeedText; 
    public float speedDivider = 1f; 
    private Rigidbody2D rb;
    private float moveSpeed = 5f; 

    private void Start()
    {
        if (centerObject == null)
        {
            Debug.LogError("Center object is not assigned!");
            return;
        }

        rb = GetComponent<Rigidbody2D>(); 

       
        Vector2 moveDirection = (centerObject.position - transform.position).normalized;

        
        rb.velocity = moveDirection * moveSpeed;

        
        UpdateMoveSpeedFromTextMeshPro();
    }

    private void FixedUpdate()
    {
        
        if (centerObject != null)
        {
            Vector2 moveDirection = (centerObject.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;

            
            UpdateMoveSpeedFromTextMeshPro();
        }
    }

    private void UpdateMoveSpeedFromTextMeshPro()
    {
       
        if (moveSpeedText != null)
        {
           
            if (float.TryParse(moveSpeedText.text, out float newMoveSpeed))
            {
                moveSpeed = newMoveSpeed / speedDivider; 
            }
            else
            {
                Debug.LogWarning("Failed to parse move speed from TextMeshProUGUI!");
            }
        }
    }
}
