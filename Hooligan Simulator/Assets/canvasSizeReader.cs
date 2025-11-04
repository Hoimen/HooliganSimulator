using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasHeightDisplay : MonoBehaviour
{
    public Canvas canvas;  // Reference to the Canvas 
    public TextMeshProUGUI heightText;  // Reference to the TextMeshProUGUII

    private CanvasScaler canvasScaler;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas reference is not assigned.");
            return;
        }

        if (heightText == null)
        {
            Debug.LogError("TextMeshProUGUI reference is not assigned.");
            return;
        }

        
        canvasScaler = canvas.GetComponent<CanvasScaler>();
    }

    void Update()
    {
        if (canvasScaler != null && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
        {
           
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            
            float canvasHeight = canvasRect.rect.height;

            
            float referenceHeight = canvasScaler.referenceResolution.y;
            float scaleFactor = Screen.height / referenceHeight;

            
            canvasHeight *= scaleFactor;

            // Update the TextMeshPro with the new height
            heightText.text = canvasHeight.ToString("F2"); 
        }
    }
}
