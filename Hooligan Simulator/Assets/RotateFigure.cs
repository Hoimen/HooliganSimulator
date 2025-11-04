using UnityEngine;
using UnityEngine.EventSystems;

public class MouseRotateWithSpecificButtonHover : MonoBehaviour
{
    public GameObject targetObject; 
    public GameObject hoverButton; 
    public float rotationSpeed = 5f; 

    private bool isMouseOverButton = false; 
    private bool isDragging = false; 

    private void Update()
    {
        
        isMouseOverButton = hoverButton != null && EventSystem.current.IsPointerOverGameObject();

       
        if (!isMouseOverButton)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }

            
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            
            if (isDragging && targetObject != null)
            {
                float mouseDeltaX = Input.GetAxis("Mouse X"); // Get horizontal mouse movement
                targetObject.transform.Rotate(0f, -mouseDeltaX * rotationSpeed, 0f, Space.World); // Invert rotation direction
            }
        }
        else
        {
            isDragging = false; 
        }
    }
}
