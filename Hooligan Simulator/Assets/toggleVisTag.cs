using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;

public class GameObjectVisibilityToggle : MonoBehaviour
{
    public List<GameObject> objectsToToggle; 
    public Button toggleButton;
    public Image displayImage; 
    public Color onColor = Color.green; 
    public Color offColor = Color.red;  

    private bool areObjectsVisible = false; 

    private void Start()
    {
       
        if (toggleButton != null && objectsToToggle.Count > 0)
        {
            toggleButton.onClick.AddListener(ToggleVisibility);
            UpdateDisplayImage(); 
        }
    }

   
    private void ToggleVisibility()
    {
        
        bool allActive = true;
        foreach (var obj in objectsToToggle)
        {
            if (!obj.activeSelf)
            {
                allActive = false;
                break;
            }
        }

       
        bool newState = !allActive;

      
        foreach (var obj in objectsToToggle)
        {
            obj.SetActive(newState);
        }

       
        UpdateDisplayImage();
    }

   
    private void UpdateDisplayImage()
    {
        if (objectsToToggle.Count > 0)
        {
           
            bool allActive = true;
            foreach (var obj in objectsToToggle)
            {
                if (!obj.activeSelf)
                {
                    allActive = false;
                    break;
                }
            }

          
            if (displayImage != null)
            {
                displayImage.color = allActive ? onColor : offColor;
            }
        }
    }
}
