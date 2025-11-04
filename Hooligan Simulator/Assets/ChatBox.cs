using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputToText : MonoBehaviour
{
    public InputField inputField;
    private TMP_Text displayText; 
    public Image textBoxImage; 
    public float fadeDuration = 1f; 
    public float waitTime = 10f; 

    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        
        displayText = GetComponent<TMP_Text>();

        
        textBoxImage.color = new Color(textBoxImage.color.r, textBoxImage.color.g, textBoxImage.color.b, 0);

        if (inputField != null)
        {
            
            inputField.onSubmit.AddListener(OnSubmit);
        }
    }

    void Update()
    {
        
        if (isFading)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                float fadeProgress = (timer - waitTime) / fadeDuration;
                textBoxImage.color = new Color(textBoxImage.color.r, textBoxImage.color.g, textBoxImage.color.b, Mathf.Lerp(1, 0, fadeProgress));

                if (fadeProgress >= 1f)
                {
                    isFading = false;
                   
                    displayText.text = "";
                }
            }
        }
    }

    void OnSubmit(string text)
    {
        if (displayText != null)
        {
            
            displayText.text = text;
        }

        
        inputField.text = "";

        
        timer = 0f;
        isFading = true;
        textBoxImage.color = new Color(textBoxImage.color.r, textBoxImage.color.g, textBoxImage.color.b, 1);
    }

    void OnDestroy()
    {
        if (inputField != null)
        {
            
            inputField.onSubmit.RemoveListener(OnSubmit);
        }
    }
}
