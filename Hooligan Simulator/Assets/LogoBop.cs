using System.Collections;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("The maximum size the panel will grow to.")]
    public float maxSize = 1.5f;

    [Tooltip("Speed at which the panel grows.")]
    public float growSpeed = 2f;

    [Tooltip("Speed at which the panel shrinks.")]
    public float shrinkSpeed = 2f;

    [Header("Panel Settings")]
    [Tooltip("The original size of the panel (set automatically if left blank).")]
    public Vector3 originalScale;

    private void Start()
    {
       
        if (originalScale == Vector3.zero)
            originalScale = transform.localScale;

       
        StartCoroutine(AnimatePanel());
    }

    private IEnumerator AnimatePanel()
    {
        while (true)
        {
            
            while (transform.localScale.x < maxSize)
            {
                transform.localScale += Vector3.one * Time.deltaTime * growSpeed;
                yield return null;
            }

           
            while (transform.localScale.x > originalScale.x)
            {
                transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;
                yield return null;
            }
        }
    }
}

