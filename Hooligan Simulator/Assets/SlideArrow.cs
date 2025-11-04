using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverSlideEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Button button; 
    public RectTransform objectToMove1; 
    public RectTransform objectToMove2;
    public float slideAmount = 100f; 
    public float moveSpeed = 5f; 

    public Color hoverColor = Color.red; 
    public Color clickColor = Color.black; 
    private Color originalColor; 

    private Vector2 originalPosition1;
    private Vector2 originalPosition2;
    private Vector2 targetPosition1;
    private Vector2 targetPosition2;
    private bool isHovering = false;
    private bool isClicked = false;

    private Image image1;
    private Image image2;

    void Start()
    {
        
        originalPosition1 = objectToMove1.anchoredPosition;
        originalPosition2 = objectToMove2.anchoredPosition;

       
        targetPosition1 = originalPosition1 - new Vector2(slideAmount, 0);
        targetPosition2 = originalPosition2 - new Vector2(slideAmount, 0);

       
        image1 = objectToMove1.GetComponent<Image>();
        image2 = objectToMove2.GetComponent<Image>();

       
        if (image1 != null)
            originalColor = image1.color;
    }

    void Update()
    {
        
        if (isClicked)
        {
            SetObjectColors(clickColor);
        }
        else if (isHovering)
        {
            SetObjectColors(hoverColor);
            MoveObjects(targetPosition1, targetPosition2);
        }
        else
        {
            SetObjectColors(originalColor); 
            MoveObjects(originalPosition1, originalPosition2);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        SetObjectColors(clickColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        SetObjectColors(hoverColor);
    }

    private void SetObjectColors(Color color)
    {
        if (image1 != null)
            image1.color = color;
        if (image2 != null)
            image2.color = color;
    }

    void MoveObjects(Vector2 pos1, Vector2 pos2)
    {
       
        objectToMove1.anchoredPosition = Vector2.Lerp(objectToMove1.anchoredPosition, pos1, Time.deltaTime * moveSpeed);
        objectToMove2.anchoredPosition = Vector2.Lerp(objectToMove2.anchoredPosition, pos2, Time.deltaTime * moveSpeed);
    }
}
