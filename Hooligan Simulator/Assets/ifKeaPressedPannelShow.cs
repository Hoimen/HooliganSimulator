using UnityEngine;

public class IfKeaPressedPannelShow : MonoBehaviour // put this in all unity projects usefull script
{
    public GameObject[] objectsToActivate; 
    public GameObject[] objectsToHide; 
    public GameObject[] objectsToToggle; // toggle on/off
    public KeyCode[] activationKeys; 

    void Update()
    {
        foreach (KeyCode key in activationKeys)
        {
            if (Input.GetKeyDown(key))
            {
                // Activate objects
                foreach (GameObject obj in objectsToActivate)
                {
                    obj.SetActive(true);
                }

                // Hide objects
                foreach (GameObject obj in objectsToHide)
                {
                    obj.SetActive(false);
                }

                // Toggle objects
                foreach (GameObject obj in objectsToToggle)
                {
                    obj.SetActive(!obj.activeSelf);
                }

                
                break;
            }
        }
    }
}
