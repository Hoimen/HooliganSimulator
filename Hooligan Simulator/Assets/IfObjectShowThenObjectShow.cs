using UnityEngine;
using System.Collections.Generic;

public class VisibilityChecker : MonoBehaviour
{
    public List<GameObject> itemsToCheckVisibility; 
    public GameObject hideableObject; 
    public List<GameObject> itemsToCheckVisibility2; 
    public GameObject hideableObject2; 

    private List<bool> wasItemVisible; 
    private List<bool> wasItemVisible2;

    void Start()
    {
        if (itemsToCheckVisibility == null || itemsToCheckVisibility.Count == 0)
        {
            Debug.LogError("No items assigned to check visibility for hideableObject!");
            return;
        }

        if (itemsToCheckVisibility2 == null || itemsToCheckVisibility2.Count == 0)
        {
            Debug.LogError("No items assigned to check visibility for hideableObject2!");
            return;
        }

        
        wasItemVisible = new List<bool>();
        wasItemVisible2 = new List<bool>();

        foreach (var item in itemsToCheckVisibility)
        {
            wasItemVisible.Add(item.activeInHierarchy); 
        }

        foreach (var item in itemsToCheckVisibility2)
        {
            wasItemVisible2.Add(item.activeInHierarchy); 
        }

        
        ToggleHideableObjects();
    }

    void Update()
    {
        
        for (int i = 0; i < itemsToCheckVisibility.Count; i++)
        {
            bool isCurrentlyVisible = itemsToCheckVisibility[i].activeInHierarchy;

            if (isCurrentlyVisible != wasItemVisible[i])
            {
                wasItemVisible[i] = isCurrentlyVisible; 
                ToggleHideableObjects(); 
            }
        }

        
        for (int i = 0; i < itemsToCheckVisibility2.Count; i++)
        {
            bool isCurrentlyVisible = itemsToCheckVisibility2[i].activeInHierarchy;

            if (isCurrentlyVisible != wasItemVisible2[i])
            {
                wasItemVisible2[i] = isCurrentlyVisible; 
                ToggleHideableObjects(); 
            }
        }
    }

    void ToggleHideableObjects()
    {
        bool anyItemVisible = false;
        bool anyItemVisible2 = false;

        
        foreach (var item in itemsToCheckVisibility)
        {
            if (item.activeInHierarchy)
            {
                anyItemVisible = true;
                break;
            }
        }

        
        foreach (var item in itemsToCheckVisibility2)
        {
            if (item.activeInHierarchy)
            {
                anyItemVisible2 = true;
                break;
            }
        }

        
        if (anyItemVisible)
        {
            if (hideableObject != null)
            {
                hideableObject.SetActive(false);
            }
        }
        else
        {
            if (hideableObject != null)
            {
                hideableObject.SetActive(true);
            }
        }

        
        if (anyItemVisible2)
        {
            if (hideableObject != null)
            {
                hideableObject.SetActive(false);
            }

            if (hideableObject2 != null)
            {
                hideableObject2.SetActive(false);
            }
        }
        else
        {
            if (hideableObject2 != null)
            {
                hideableObject2.SetActive(true);
            }
        }
    }
}
