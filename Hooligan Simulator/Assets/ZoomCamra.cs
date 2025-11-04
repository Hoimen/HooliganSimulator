using UnityEngine;

public class ScrollCameraZ : MonoBehaviour
{
    [Header("Camera Settings")]
    public GameObject cameraObject;       
    public float scrollSensitivity = 1f; 
    public float minDistance = 1f;       
    public float maxDistance = 20f;      

    private void Update()
    {
       
        if (cameraObject == null)
        {
            Debug.LogWarning("Camera object is not assigned!");
            return;

        }

       
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
           
            Vector3 origin = cameraObject.transform.parent != null
                ? cameraObject.transform.parent.position
                : Vector3.zero;

           
            Vector3 direction = (cameraObject.transform.position - origin).normalized;

           
            float currentDistance = Vector3.Distance(cameraObject.transform.position, origin);
            float newDistance = Mathf.Clamp(currentDistance - scroll * scrollSensitivity, minDistance, maxDistance);

           
            cameraObject.transform.position = origin + direction * newDistance;
        }
    }
}
