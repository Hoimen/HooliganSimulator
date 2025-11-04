using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Scene Switching")]
    public Button switchSceneButton;
    public string sceneToLoad;
    public float sceneLoadDelay = 0f;

    [Header("Optional Fade Settings")]
    public bool useFade = false;
    public Image fadeImage1;   // First fade image
    public Image fadeImage2;   // Second fade image
    public float fadeDuration = 1f;
    public float fadeStartDelay = 0f; // Delay before fade begins

    void Start()
    {
        if (switchSceneButton != null)
        {
            switchSceneButton.onClick.AddListener(OnSwitchScene);
        }

     
        if (useFade)
        {
            if (fadeImage1 != null)
            {
                SetFadeAlpha(fadeImage1, 0f);
                fadeImage1.raycastTarget = false;
            }
            if (fadeImage2 != null)
            {
                SetFadeAlpha(fadeImage2, 0f);
                fadeImage2.raycastTarget = false;
            }
        }
    }

    void OnSwitchScene()
    {
        StartCoroutine(LoadSceneDelayed());
    }

    IEnumerator LoadSceneDelayed()
    {
        if (useFade && (fadeImage1 != null || fadeImage2 != null))
        {
            // Wait before starting the fade
            if (fadeStartDelay > 0f)
                yield return new WaitForSeconds(fadeStartDelay);

            
            yield return StartCoroutine(Fade(0f, 1f));
        }

       
        if (sceneLoadDelay > 0f)
            yield return new WaitForSeconds(sceneLoadDelay);

        // Load the scene
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);

           
            if (fadeImage1 != null)
            {
                Color color1 = fadeImage1.color;
                color1.a = alpha;
                fadeImage1.color = color1;
            }
            if (fadeImage2 != null)
            {
                Color color2 = fadeImage2.color;
                color2.a = alpha;
                fadeImage2.color = color2;
            }
            yield return null;
        }

       
        if (fadeImage1 != null)
        {
            Color color1 = fadeImage1.color;
            color1.a = to;
            fadeImage1.color = color1;
        }
        if (fadeImage2 != null)
        {
            Color color2 = fadeImage2.color;
            color2.a = to;
            fadeImage2.color = color2;
        }
    }

    void SetFadeAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }
}
