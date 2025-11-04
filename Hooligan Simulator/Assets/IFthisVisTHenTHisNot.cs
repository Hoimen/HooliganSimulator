using UnityEngine;
using System.Collections.Generic;

public class VisibilityControlWithUI : MonoBehaviour
{
    
    public List<GameObject> objectsToCheck; 
    public GameObject playerUi;  

    private void Update()
    {
        
        bool allInvisible = true;

        foreach (GameObject obj in objectsToCheck)
        {
            
            if (obj.activeSelf)
            {
                allInvisible = false;
                break;  
            }
        }

       
        playerUi.SetActive(allInvisible);
    }
}
