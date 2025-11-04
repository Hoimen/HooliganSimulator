using TMPro.Examples;
using UnityEngine;

public class MouseLockController : MonoBehaviour
{
    public GameObject thirdPersonPanel; 
    public GameObject firstPersonUIPanel; 
    private CameraController cameraController; 

    private bool cursorPreviouslyLocked; 

    void Start()
    {
        
        cameraController = GetComponent<CameraController>(); 
        cursorPreviouslyLocked = false; 
    }

    void Update()
    {
        
        if (firstPersonUIPanel.activeSelf)
        {
            LockCursorForFirstPersonUI();
        }
        
        else if (thirdPersonPanel.activeSelf)
        {
            
            if (Input.GetMouseButton(1)) 
            {
                LockCursorToCenter();
                EnableCameraControl(true); 
            }
            else
            {
                
                EnableCameraControl(false);
                UnlockCursor(); 
            }
        }
        else
        {
            
            LockCursorToCenter();
            EnableCameraControl(true);
        }
    }

    
    private void LockCursorToCenter()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        cursorPreviouslyLocked = true; 
    }

    
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
        cursorPreviouslyLocked = false; 
    }

    
    private void LockCursorForFirstPersonUI()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        EnableCameraControl(true); 
    }

    
    private void EnableCameraControl(bool enable)
    {
        if (cameraController != null)
        {
            cameraController.enabled = enable; 
        }
    }
}
