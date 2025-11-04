using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderToggle : MonoBehaviour
{
    public Slider slider;                     
    public KeyCode toggleKey = KeyCode.R;     
    public float animationDuration = 0.5f;    

    public GameObject objectToShow;           
    public GameObject objectToHide;           

    public Button onScreenButton;             

    private bool isSliderAtMax = false;      
    private Coroutine animationCoroutine;

    void Start()
    {
       
        if (onScreenButton != null)
        {
            onScreenButton.onClick.AddListener(ToggleSlider);
        }

        
        UpdateObjectVisibility();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(toggleKey)) // R
        {
            ToggleSlider();
        }
    }

    void ToggleSlider()
    {
        
        isSliderAtMax = !isSliderAtMax;
        float targetValue = isSliderAtMax ? 1 : 0;

        
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        
        animationCoroutine = StartCoroutine(AnimateSlider(targetValue));
    }

    private IEnumerator AnimateSlider(float targetValue)
    {
        float startValue = slider.value;
        float timeElapsed = 0f;

        
        while (timeElapsed < animationDuration)
        {
            slider.value = Mathf.Lerp(startValue, targetValue, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        slider.value = targetValue;

       
        UpdateObjectVisibility();
    }

    private void UpdateObjectVisibility()
    {
        
        if (objectToShow != null)
        {
            objectToShow.SetActive(slider.value >= 1);
        }
        if (objectToHide != null)
        {
            objectToHide.SetActive(slider.value < 1);
        }
    }
}
