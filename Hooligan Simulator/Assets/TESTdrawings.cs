using UnityEngine;

public class Painter : MonoBehaviour
{
    [Header("Paint Settings")]
    public GameObject paintPrefab;  
    public float paintSize = 0.1f;  
    public LayerMask paintableLayers;  

    [Header("Camera Settings")]
    public Camera targetCamera;  

    [Header("Input Settings")]
    public KeyCode paintKey = KeyCode.K; // kea for paint is "k"

    void Update()
    {
        //camra
        if (targetCamera == null)
        {
            Debug.LogWarning("No camera assigned to Painter script.");
            return;
        }

       
        if (Input.GetKey(paintKey))
        {
            // raycast bruh
            Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, paintableLayers))
            {
               
                GameObject paintMark = Instantiate(paintPrefab, hit.point, Quaternion.identity);

               
                paintMark.transform.localScale = Vector3.one * paintSize;

               
                paintMark.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
}
