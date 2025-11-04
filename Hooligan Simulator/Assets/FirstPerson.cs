using UnityEngine;

public class CameraFovBehavior : MonoBehaviour
{
    public Camera mainCamera; //camra lol
    public GameObject objectToHide1;
    public GameObject objectToHide2;
    public GameObject objectToShow1;
    public GameObject objectToShow2;

    private float defaultFov;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }

        
        defaultFov = mainCamera.fieldOfView;

        
        objectToShow1.SetActive(true);
        objectToShow2.SetActive(true);
        objectToHide1.SetActive(true);
        objectToHide2.SetActive(true);
    }

    void Update()
    {
        // Check if the camera's FOV is 12
        if (mainCamera.fieldOfView == 12f)
        {
            
            objectToHide1.SetActive(false);
            objectToHide2.SetActive(false);
            objectToShow1.SetActive(true);
            objectToShow2.SetActive(true);
        }
        else
        {
            
            objectToHide1.SetActive(true);
            objectToHide2.SetActive(true);
            objectToShow1.SetActive(false);
            objectToShow2.SetActive(false);
        }
    }
}
