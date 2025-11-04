using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("The button this script will interact with.")]
    public Button buttonObject; 

    [Tooltip("The GameObject to animate when hovering.")]
    public GameObject objectToShow; 

    [Tooltip("The GameObject to check visibility status.")]
    public GameObject objectToCheck; 

    public float slideDistance = 50f; 
    public float animationSpeed = 5f; 
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isHovered = false;

    private void Start()
    {
        if (buttonObject == null)
        {
            Debug.LogError("Button Object is not assigned. Please assign it in the Inspector.", this);
        }

        if (objectToShow != null)
        {
            initialPosition = objectToShow.transform.localPosition;
            targetPosition = initialPosition + new Vector3(0, slideDistance, 0);
        }

        if (objectToCheck == null)
        {
            Debug.LogWarning("Object to Check is not assigned. The sliding behavior will rely only on hover state.", this);
        }
    }

    private void Update()
    {
        if (objectToShow != null)
        {
            
            bool shouldSlideUp = isHovered || (objectToCheck != null && objectToCheck.activeSelf);

            objectToShow.transform.localPosition = Vector3.Lerp(
                objectToShow.transform.localPosition,
                shouldSlideUp ? targetPosition : initialPosition,
                Time.deltaTime * animationSpeed
            );
        }
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
