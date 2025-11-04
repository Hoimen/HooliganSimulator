using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    public Image targetImage;   
    public Color color1 = Color.white;  // Initial color
    public Color color2 = Color.red;    // Second color

    private bool isColor1 = true;   // Flag to track current color

    public void ToggleColor()
    {
        if (isColor1)
        {
            targetImage.color = color2;
        }
        else
        {
            targetImage.color = color1;
        }

        isColor1 = !isColor1;   
    }
}
