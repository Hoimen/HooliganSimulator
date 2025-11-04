using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image imageToHover; 
    public Image[] imagesToChange; 
    public Color hoverColor = Color.red; // Color when hovered over
    public Color clickColor = Color.black; // Color when clicked

    private Color[] originalColors; 

    void Start()
    {
        
        originalColors = new Color[imagesToChange.Length];

        
        for (int i = 0; i < imagesToChange.Length; i++)
        {
            originalColors[i] = imagesToChange[i].color;
        }
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColor(hoverColor);
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        RestoreOriginalColors();
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        ChangeColor(clickColor);
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        ChangeColor(hoverColor);
    }

    
    private void ChangeColor(Color color)
    {
        foreach (Image img in imagesToChange)
        {
            img.color = color;
        }
    }

    
    private void RestoreOriginalColors()
    {
        for (int i = 0; i < imagesToChange.Length; i++)
        {
            imagesToChange[i].color = originalColors[i];
        }
    }
}
