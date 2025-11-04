using UnityEngine;

public class RotatingBobbingCanvas : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public RotationAxis rotationAxis = RotationAxis.Y; 
    public float rotationSpeed = 10f; // Speed of rotation
    public float bobbingHeight = 0.1f; // Height for the bobbing effect
    public float bobbingSpeed = 1f; // Speed of the bobbing effect
    public float bobbingOffset = 0f; // Initial offset for bobbing effect

    private Vector3 initialLocalPosition;

    void Start()
    {
        
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {
        
        RotateCanvas();

       
        BobbingEffect();

        
        FollowParent();
    }

    void RotateCanvas()
    {
        
        float rotationAmount = rotationSpeed * Time.deltaTime;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                transform.Rotate(rotationAmount, 0, 0);
                break;
            case RotationAxis.Y:
                transform.Rotate(0, rotationAmount, 0);
                break;
            case RotationAxis.Z:
                transform.Rotate(0, 0, rotationAmount);
                break;
        }
    }

    void BobbingEffect()
    {
        
        float newYPosition = initialLocalPosition.y + Mathf.Sin(Time.time * bobbingSpeed + bobbingOffset) * bobbingHeight;
        transform.localPosition = new Vector3(initialLocalPosition.x, newYPosition, initialLocalPosition.z);
    }

    void FollowParent()
    {
        
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + transform.localPosition;
        }
    }
}
