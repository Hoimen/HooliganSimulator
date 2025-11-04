using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f; // Time between updates (in seconds)

    private float timeSinceLastUpdate = 0f;
    private float deltaTime = 0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            float fps = 1.0f / deltaTime;
            fpsText.text = "Version 1.1 FPS: " + Mathf.Ceil(fps).ToString();
            timeSinceLastUpdate = 0f;
        }
    }
}
