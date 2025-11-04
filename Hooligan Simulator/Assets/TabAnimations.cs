using UnityEngine;
using System.Collections;

public class PanelAnimationLoop : MonoBehaviour
{
    public RectTransform panel1;
    public RectTransform panel2;
    public RectTransform panel3;

    public float slideDuration = 1.0f; 
    public float slideDistanceUp = 100.0f; 

    private Vector2 originalPos1;
    private Vector2 originalPos2;
    private Vector2 originalPos3;

    private void Start()
    {
       
        originalPos1 = panel1.anchoredPosition;
        originalPos2 = panel2.anchoredPosition;
        originalPos3 = panel3.anchoredPosition;

        
        StartCoroutine(PanelLoop());
    }

    IEnumerator PanelLoop()
    {
        while (true)
        {
           
            yield return StartCoroutine(SlidePanel(panel1, originalPos1 + Vector2.up * slideDistanceUp, originalPos1));

            
            yield return StartCoroutine(SlidePanel(panel2, originalPos2 + Vector2.up * slideDistanceUp, originalPos2));

           
            yield return StartCoroutine(SlidePanel(panel3, originalPos3 + Vector2.up * slideDistanceUp, originalPos3));
        }
    }

    IEnumerator SlidePanel(RectTransform panel, Vector2 targetUp, Vector2 targetDown)
    {
       
        yield return StartCoroutine(Slide(panel, targetUp));

        
        yield return StartCoroutine(Slide(panel, targetDown));
    }

    IEnumerator Slide(RectTransform panel, Vector2 targetPosition)
    {
        float startTime = Time.time;
        Vector2 startPos = panel.anchoredPosition;

        while (Time.time < startTime + slideDuration)
        {
            float t = (Time.time - startTime) / slideDuration;
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        
        panel.anchoredPosition = targetPosition;
    }
}
