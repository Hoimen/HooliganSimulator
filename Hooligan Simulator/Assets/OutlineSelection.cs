using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOutlineSelection : MonoBehaviour
{
    private Transform currentHighlightedObject;
    private RaycastHit hit;

    [SerializeField] private Color outlineColor = Color.magenta;
    [SerializeField] private float outlineWidth = 7.0f;
    [SerializeField] private LayerMask shopOwnerLayer; 

    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, shopOwnerLayer))
        {
            Transform target = hit.transform;

          
            if (target.CompareTag("Selectable"))
            {
                
                if (target != currentHighlightedObject)
                {
                    
                    if (currentHighlightedObject != null)
                    {
                        DisableOutline(currentHighlightedObject);
                    }

                    
                    EnableOutline(target);
                    currentHighlightedObject = target;
                }
            }
            else
            {
                
                if (currentHighlightedObject != null)
                {
                    DisableOutline(currentHighlightedObject);
                    currentHighlightedObject = null;
                }
            }
        }
        else
        {
           
            if (currentHighlightedObject != null)
            {
                DisableOutline(currentHighlightedObject);
                currentHighlightedObject = null;
            }
        }
    }

    void EnableOutline(Transform target)
    {
       
        SkinnedMeshRenderer skinnedMeshRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            foreach (Transform bone in skinnedMeshRenderer.bones)
            {
                AddOutlineToTransform(bone);
            }
        }
        else
        {
            
            AddOutlineToTransform(target);
        }
    }

    void AddOutlineToTransform(Transform transform)
    {
        var outline = transform.GetComponent<Outline>();
        if (outline == null)
        {
            outline = transform.gameObject.AddComponent<Outline>();
        }
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = true;
    }

    void DisableOutline(Transform target)
    {
        var outline = target.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
