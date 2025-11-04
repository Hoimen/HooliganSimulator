using UnityEngine;
using TMPro;

public class PlacePointsOnCircle : MonoBehaviour
{
    public GameObject objectType1;
    public GameObject objectType2; 
    public GameObject objectType3;
    public GameObject objectType4;
    public GameObject objectType5;
    public GameObject objectType6;

    [Tooltip("Probability (%) of spawning each object type")]
    public float[] spawnProbabilities = { 30f, 25f, 20f, 25f }; 

    public int numPoints = 50;
    public float radius = 400f;
    public float spawnInterval = 1f; 
    public float spawnIntervalMultiplier = 1f; 

   
    [Header("Multiplier Settings")]
    public float spawnIntervalIncrease = 0.2f; 
    public float multiplierIncreasePeriod = 30f;
    public float spawnIntervalDecreaseRate = 0.05f; 

    private float timeSinceLastSpawn;
    private float timeSinceLastMultiplierUpdate;
    private Vector3[] points;
    private float currentSpawnIntervalMultiplier;

  
    public TextMeshProUGUI timerText1;     
    public TextMeshProUGUI timerText2;     

  
    public TextMeshProUGUI multiplierText1;
    public TextMeshProUGUI multiplierText2; 

    private float stopwatchTime; // Time in seconds for stopwatch
    private int hours, minutes, seconds, hundredths;

    void Start()
    {
        timeSinceLastSpawn = spawnInterval;
        timeSinceLastMultiplierUpdate = 0f;// time multiplier 
        currentSpawnIntervalMultiplier = 1f; // Initial multiplier
        stopwatchTime = 0f;

        points = GenerateCirclePoints();

     
        timerText1.text = "";
        timerText2.text = "";
        multiplierText1.text = FormatMultiplier(currentSpawnIntervalMultiplier);  // Initial multiplier
        multiplierText2.text = FormatMultiplier(currentSpawnIntervalMultiplier);  // Initial multiplier
    }

    void Update()
    {
        UpdateStopwatch();
        UpdateSpawnInterval();
        TrySpawnObject();  
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

       
        hours = Mathf.FloorToInt(stopwatchTime / 3600f);
        minutes = Mathf.FloorToInt((stopwatchTime % 3600f) / 60f);
        seconds = Mathf.FloorToInt(stopwatchTime % 60f);
        hundredths = Mathf.FloorToInt((stopwatchTime * 100f) % 100f);

        string formattedTime = "";

       
        if (hours > 0)
        {
            formattedTime += string.Format("{0:00}:", hours);
        }

       
        if (minutes > 0 || hours > 0)
        {
            formattedTime += string.Format("{0:00}:", minutes);
        }

      
        formattedTime += string.Format("{0:00}.", seconds);

        
        formattedTime += string.Format("{0:00}", hundredths);

       
        timerText1.text = formattedTime;
        timerText2.text = formattedTime;
    }


    // ask gpt to do the rest



    void UpdateSpawnInterval()
    {
        timeSinceLastMultiplierUpdate += Time.deltaTime;

        // Check if it's time to update the spawn interval multiplier (every multiplierIncreasePeriod seconds)
        if (timeSinceLastMultiplierUpdate >= multiplierIncreasePeriod)
        {
            currentSpawnIntervalMultiplier += spawnIntervalIncrease; // Add .2 to the multiplier
            spawnInterval -= spawnIntervalDecreaseRate; // Decrease the spawn interval (this makes the spawn rate faster)

            // Make sure the spawn interval doesn't go below a minimum threshold
            if (spawnInterval < 0.1f)
            {
                spawnInterval = 0.1f; // Set a lower limit on the spawn interval
            }

            // Update multiplier UI text with the formatted multiplier
            string multiplierText = FormatMultiplier(currentSpawnIntervalMultiplier);
            multiplierText1.text = multiplierText;
            multiplierText2.text = multiplierText;

            timeSinceLastMultiplierUpdate = 0f;
        }
    }

    // Function to format the multiplier to remove unnecessary trailing zeros
    string FormatMultiplier(float multiplier)
    {
        if (multiplier % 1 == 0)
        {
            // No decimal part, show as integer (without decimals)
            return string.Format("x{0:0}", multiplier);
        }
        else
        {
            // Show up to two decimal places, removing unnecessary trailing zeros
            return string.Format("x{0:0.##}", multiplier);
        }
    }

    void TrySpawnObject()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Spawn object if enough time has passed based on adjusted spawn interval
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObject();
            timeSinceLastSpawn = 0f; // Reset spawn timer
        }
    }

    void SpawnObject()
    {
        if (points.Length == 0)
        {
            Debug.LogError("No points generated.");
            return;
        }

        // Calculate total probability sum
        float totalProbability = 0f;
        foreach (float prob in spawnProbabilities)
        {
            totalProbability += prob;
        }

        // Randomly select an object type based on probabilities
        float randomValue = Random.Range(0f, totalProbability);
        GameObject objectToSpawn = null;

        float cumulativeProbability = 0f;
        for (int i = 0; i < spawnProbabilities.Length; i++)
        {
            cumulativeProbability += spawnProbabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                // Assign the correct object type based on index
                switch (i)
                {
                    case 0:
                        objectToSpawn = objectType1;
                        break;
                    case 1:
                        objectToSpawn = objectType2;
                        break;
                    case 2:
                        objectToSpawn = objectType3;
                        break;
                    case 3:
                        objectToSpawn = objectType4;
                        break;
                    case 4:
                        objectToSpawn = objectType5;
                        break;
                    case 5:
                        objectToSpawn = objectType6;
                        break;
                    default:
                        Debug.LogError("Invalid object type index.");
                        return;
                }
                break;
            }
        }

        if (objectToSpawn == null)
        {
            Debug.LogError("Object to spawn is not assigned.");
            return;
        }

        // Randomly select a point from the circle to spawn at
        int randomIndex = Random.Range(0, points.Length);
        Vector3 spawnPosition = points[randomIndex];

        // Spawn the object at the random position
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.transform.SetParent(transform, false);

        // Optionally, add Rigidbody2D and other components if required
        Rigidbody2D rb2d = spawnedObject.GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = spawnedObject.AddComponent<Rigidbody2D>();
        }

        AutoMoveToCenter2D moveScript = spawnedObject.GetComponent<AutoMoveToCenter2D>();
        if (moveScript == null)
        {
            moveScript = spawnedObject.AddComponent<AutoMoveToCenter2D>();
        }
    }

    Vector3[] GenerateCirclePoints()
    {
        Vector3[] circlePoints = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * (360f / numPoints);
            float radianAngle = Mathf.Deg2Rad * angle;

            Vector3 position = new Vector3(
                radius * Mathf.Cos(radianAngle),
                radius * Mathf.Sin(radianAngle),
                0f
            );

            circlePoints[i] = position;
        }

        return circlePoints;
    }
}
