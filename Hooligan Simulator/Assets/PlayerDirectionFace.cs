using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionFace : MonoBehaviour
{
    public PlayerController playerController;  
    public GameObject visualPlayer;  
    public GameObject centralObject; 
    public float rotationSpeed = 5f;  

    private Vector3 lastMoveDirection;

    void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned in PlayerDirectionFace.");
        }

        if (visualPlayer == null)
        {
            Debug.LogError("VisualPlayer is not assigned in PlayerDirectionFace.");
        }

        if (centralObject == null)
        {
            Debug.LogError("CentralObject is not assigned in PlayerDirectionFace.");
        }

       
        lastMoveDirection = Vector3.zero;
    }

    void Update()
    {
        if (playerController == null || visualPlayer == null || centralObject == null)
            return;

        
        TrackMovementDirection();

        
        if (playerController.canMove)
        {
            HandleRotation();
        }
    }

    
    private void HandleRotation()
    {
        
        if (lastMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection);
            visualPlayer.transform.rotation = Quaternion.Slerp(visualPlayer.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    
    private void TrackMovementDirection()
    {
        if (playerController != null)
        {
           
            Quaternion centralRotation = Quaternion.Euler(0, centralObject.transform.eulerAngles.y, 0);

            
            Vector3 localMoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

           
            Vector3 worldMoveDirection = centralRotation * localMoveDirection;

            
            Vector3 flippedDirection = -worldMoveDirection;

           
            if (flippedDirection.magnitude > 0.1f)
            {
                lastMoveDirection = flippedDirection.normalized;
            }
        }
    }
}