using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    public Camera targetCamera; 
    public bool rotateOnlyXAxis = false; 

    void Update()
    {
        
        if (targetCamera == null) return; 

        // Ensure the canvas faces the target camera
        transform.LookAt(targetCamera.transform);

        if (rotateOnlyXAxis)
        {
            
            Vector3 targetPosition = new Vector3(targetCamera.transform.position.x, transform.position.y, transform.position.z);
            transform.LookAt(targetPosition);
        }
        else
        {
            
            transform.Rotate(0, 180, 0); 
        }
    }
}
