using UnityEngine;
using UnityEngine.UI;

public class AmbientColorChanger : MonoBehaviour
{
    public Button redButton;
    public Button blueButton;
    public Button yellowButton;
    public Button greenButton;
    public Button pinkButton;
    public Button grayButton; //defualt color

    void Start()
    {
       
        redButton.onClick.AddListener(() => ChangeAmbientColor(new Color(1.0f, 0.6f, 0.6f))); // Softer red
        blueButton.onClick.AddListener(() => ChangeAmbientColor(new Color(0.6f, 0.6f, 1.0f))); // Softer blue
        yellowButton.onClick.AddListener(() => ChangeAmbientColor(new Color(1.0f, 1.0f, 0.6f))); // Softer yellow
        greenButton.onClick.AddListener(() => ChangeAmbientColor(new Color(0.6f, 1.0f, 0.6f))); // Softer green
        pinkButton.onClick.AddListener(() => ChangeAmbientColor(new Color(1.0f, 0.5f, 1.0f))); // Softer pink
        grayButton.onClick.AddListener(() => ChangeAmbientColor(new Color(1.0f, 1.0f, 1.0f))); // Softer gray
    }

    void ChangeAmbientColor(Color newColor)
    {
        RenderSettings.ambientLight = newColor;
        Debug.Log($"Ambient color changed to: {newColor}");
    }
}
