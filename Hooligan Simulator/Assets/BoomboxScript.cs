using UnityEngine;
using UnityEngine.UI;

public class ObjectVisibilityManager : MonoBehaviour
{
    public GameObject[] toggleableObjects; 
    public Button[] buttons; // funny little aray for buttons

    private GameObject currentVisibleObject; 

    void Start()
    {
        
        foreach (var obj in toggleableObjects)
        {
            obj.SetActive(false);
        }

        // click listeners to each button
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => ToggleObject(index));
        }
    }

    void ToggleObject(int index)
    {
        
        if (index < 0 || index >= toggleableObjects.Length)
        {
            Debug.LogWarning("Invalid index!");
            return;
        }

        
        if (currentVisibleObject != null)
        {
            currentVisibleObject.SetActive(false);
        }

        
        toggleableObjects[index].SetActive(true);
        currentVisibleObject = toggleableObjects[index];
    }
}
