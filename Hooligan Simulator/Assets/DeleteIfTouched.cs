using UnityEngine;
using TMPro;

public class CollisionHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreTextTopLeft;      
    public TextMeshProUGUI scoreTextBottomRight;  
    private int score = 0;                        
    private int pointsToAdd = 0;                  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Despawnthis_25points"))
        {
            Destroy(collision.gameObject);  
            pointsToAdd += 25;              // Add 25 points
        }
        else if (collision.gameObject.CompareTag("Despawnthis_50points"))
        {
            Destroy(collision.gameObject);  
            pointsToAdd += 50;              // Add 50 points
        }
        else if (collision.gameObject.CompareTag("Despawnthis_100points"))
        {
            Destroy(collision.gameObject);  
            pointsToAdd += 100;             // Add 100 points
        }
        else if (collision.gameObject.CompareTag("Despawnthis_200points"))
        {
            Destroy(collision.gameObject);  
            pointsToAdd += 200;             // Add 200 points
        }

        
        if (pointsToAdd > 0)
        {
            score += pointsToAdd;  
            StartCoroutine(IncrementScore());  
            pointsToAdd = 0;  
        }
    }

    
    private System.Collections.IEnumerator IncrementScore()
    {
        int startScore = score - pointsToAdd;  
        int endScore = score;
        float duration = 0.5f;  

        float startTime = Time.time;
        float endTime = startTime + duration;

        // Animate the score smoothly becuase it looks better
        while (Time.time < endTime)
        {
            float timeRatio = (Time.time - startTime) / duration;
            int animatedScore = (int)Mathf.Lerp(startScore, endScore, timeRatio);  
            UpdateScoreText(animatedScore);  
            yield return null;
        }

        UpdateScoreText(endScore);  
    }

    
    void UpdateScoreText(int animatedScore)
    {
        if (scoreTextTopLeft != null)
        {
            scoreTextTopLeft.text = animatedScore.ToString();  
        }

        if (scoreTextBottomRight != null)
        {
            scoreTextBottomRight.text = animatedScore.ToString();  
        }
    }
}
