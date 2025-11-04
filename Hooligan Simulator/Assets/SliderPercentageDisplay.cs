using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderPercentageDisplay : MonoBehaviour
{
   
    public Slider slider;
   
    public TextMeshProUGUI percentageText;

    void Start()
    {
        
        UpdatePercentageText();
        slider.onValueChanged.AddListener(delegate { UpdatePercentageText(); });
    }

   
    void UpdatePercentageText()
    {
        
        int percentage = Mathf.RoundToInt(slider.value * 100);
        
        percentageText.text = percentage.ToString() + "";
    }

    void OnDestroy()
    {
       
        slider.onValueChanged.RemoveListener(delegate { UpdatePercentageText(); });
    }
}
