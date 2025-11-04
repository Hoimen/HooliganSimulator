using UnityEngine;

public class SprayPaintVisibility : MonoBehaviour
{
    public GameObject sprayPaint; 

    
    void Start()
    {
        
        sprayPaint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetMouseButton(0)) 
        {
            sprayPaint.SetActive(true); 
        }
        else
        {
            sprayPaint.SetActive(false); 
        }
    }
}
