using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckTextBoxGrowth : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; 
    public RectTransform moveImage; 
    public float moveAmount = 50f;

    private float previousHeight;

    void Start()
    {
        
        previousHeight = textMeshPro.rectTransform.rect.height;
    }

    void Update()
    {
       
        float currentHeight = textMeshPro.rectTransform.rect.height;

        
        if (currentHeight > previousHeight)
        {
            int linesAdded = Mathf.RoundToInt((currentHeight - previousHeight) / textMeshPro.fontSize);
            Debug.Log($"The text box has grown in height by {linesAdded} rows.");
            MoveImageUp(linesAdded);
            previousHeight = currentHeight; 
        }
        
        else if (currentHeight < previousHeight)
        {
            int linesRemoved = Mathf.RoundToInt((previousHeight - currentHeight) / textMeshPro.fontSize);
            Debug.Log($"The text box has shrunk in height by {linesRemoved} rows.");
            MoveImageDown(linesRemoved);
            previousHeight = currentHeight; 
        }
    }

    void MoveImageUp(int linesAdded)
    {
        
        moveImage.anchoredPosition += new Vector2(0, moveAmount * linesAdded);
    }

    void MoveImageDown(int linesRemoved)
    {
        
        moveImage.anchoredPosition -= new Vector2(0, moveAmount * linesRemoved);
    }
}
