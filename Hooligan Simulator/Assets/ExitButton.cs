using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton; 

    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    void ExitGame()
    {
        
        Application.Quit();

#if UNITY_EDITOR
        // Stops play mode in the Unity Editor.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
