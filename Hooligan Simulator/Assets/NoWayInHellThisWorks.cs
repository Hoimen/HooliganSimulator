using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SprayPainter : MonoBehaviour // NOT USED ANYMORE IMPLEMENTED INTO THE SPAWN CONTROLLER/CUBE SPAWNER, SCRIPT
{
    public GameObject paintPrefab; // Prefab for the paint mark
    public TextMeshProUGUI quadCounter; // TextMeshPro for displaying count
    public float spawnDistance = 0.01f; // Minimum distance to prevent stacking (now allows lower values)
    public float quadDistance = 0.02f; // Distance between quads while dragging (adjustable)
    public float surfaceOffset = 0.001f; // Prevents Z-fighting

    private Camera cam;
    private List<Vector3> paintPositions = new List<Vector3>(); // Stores previous quad positions
    private int quadCount = 0; // Tracks total quads spawned
    private Vector3 lastSpawnPosition; // Tracks last spawn position
    private bool hasSpawnedFirstQuad = false; // Ensures first quad always spawns

    void Start()
    {
        cam = Camera.main;
        UpdateCounter();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Hold to spray
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Paintable"))
                {
                    Vector3 hitPosition = hit.point + (hit.normal * surfaceOffset);

                    if (!hasSpawnedFirstQuad || Vector3.Distance(lastSpawnPosition, hitPosition) >= quadDistance)
                    {
                        if (!IsTooClose(hitPosition)) // Ensures no stacking
                        {
                            SpawnQuad(hitPosition, hit.normal, hit.collider.gameObject);
                            lastSpawnPosition = hitPosition;
                            hasSpawnedFirstQuad = true;
                        }
                    }
                }
            }
        }
        else
        {
            hasSpawnedFirstQuad = false; // Reset for next click
        }
    }

    void SpawnQuad(Vector3 position, Vector3 normal, GameObject surface)
    {
        // Create new paint quad
        GameObject paint = Instantiate(paintPrefab, position, Quaternion.LookRotation(-normal));
        paint.transform.SetParent(surface.transform);

        // Store position to prevent overlap
        paintPositions.Add(position);

        // Increase quad count & update counter
        quadCount++;
        UpdateCounter();
    }

    bool IsTooClose(Vector3 position)
    {
        foreach (Vector3 existingPosition in paintPositions)
        {
            if (Vector3.Distance(existingPosition, position) < spawnDistance)
                return true;
        }
        return false;
    }

    void UpdateCounter()
    {
        if (quadCounter != null)
        {
            quadCounter.text = "Quads: " + quadCount;
        }
    }
}
