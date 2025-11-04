using UnityEngine;

public class ToggleMultipleObjects : MonoBehaviour
{
    public GameObject[] objectsToAppear;
    public GameObject[] objectsToDisappear;
    public GameObject[] objectsToToggle;

    public void ToggleObjectsVisibility()
    {
        // Appear
        foreach (GameObject obj in objectsToAppear)
        {
            obj.SetActive(true);
        }

        // Disappear
        foreach (GameObject obj in objectsToDisappear)
        {
            obj.SetActive(false);
        }

        // Toggle
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
