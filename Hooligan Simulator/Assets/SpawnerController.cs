using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Alteruna;

public class CubeSpawner : MonoBehaviour
{
    public Alteruna.Avatar avatar;
    private Spawner _spawner;
    private InventoryManager inventoryManager;

    [Header("Spray Camera")]
    [SerializeField] private Camera sprayCamera;

    [Header("Debug Settings")]
    [SerializeField] private bool drawDebugRaycast = true;
    [SerializeField] private float debugRayLength = 100f;

    [Tooltip("Enable hold-to-spray for spray-mode items (must match item count)")]
    [SerializeField] private List<bool> useHoldToSpray = new List<bool>();

    [Header("Per-Item Ammo/Despawn Settings")]
    [SerializeField] private List<bool> isInfiniteItem = new List<bool>();
    [SerializeField] private List<bool> shouldDespawn = new List<bool>();

    [SerializeField] private float[] despawnTimes = new float[5];
    [SerializeField] private TextMeshProUGUI[] bulletCounters;
    [SerializeField] private int[] startingBulletCounts = new int[5];
    [SerializeField] private float[] cooldownTimes = new float[5];

    [SerializeField] private List<GameObject> settingsPanels;

    private int[] bulletCounts;
    private float[] lastShotTime;

    // New list to track which items can only have one instance at a time
    [Header("Per-Item One-at-a-Time Spawn Settings")]
    [SerializeField] private List<bool> spawnOneAtATime = new List<bool>();  // Add this list to the Inspector

    // Use a dictionary to track spawned cubes by their index
    private Dictionary<int, GameObject> spawnedCubes = new Dictionary<int, GameObject>();

    [SerializeField] private GameObject objectToSpawnInFrontOf;
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private float spawnHeight = 1f;

    [Header("Spray Settings")]
    [SerializeField] private float spraySurfaceOffset = 0.001f;
    [SerializeField] private float sprayMinDistance = 0.01f;
    [SerializeField] private float sprayQuadDistance = 0.02f;

    [Header("Per-Item Spray Mode")]
    [SerializeField] private List<bool> useSprayMode = new List<bool>();

    private List<Vector3> sprayPositions = new List<Vector3>();
    private Vector3 lastSprayPosition;
    private bool hasSpawnedSpray = false;

    private Camera cam;

    private void Awake()
    {
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        cam = Camera.main;

        bulletCounts = new int[startingBulletCounts.Length];
        lastShotTime = new float[cooldownTimes.Length];

        for (int i = 0; i < bulletCounts.Length; i++)
        {
            bulletCounts[i] = startingBulletCounts[i];
            lastShotTime[i] = -cooldownTimes[i];
            UpdateCounterText(i);
        }

        foreach (GameObject panel in settingsPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        Debug.Log("[Spawner] Awake complete. Camera cached: " + (cam != null));
    }

    private void Update()
    {
        if (!avatar.IsMe) return;

        foreach (GameObject panel in settingsPanels)
        {
            if (panel != null && panel.activeSelf)
                return;
        }

        int selectedItemIndex = inventoryManager.GetSelectedItemIndex();
        if (selectedItemIndex == -1)
            return;

        bool isSpray = useSprayMode.Count > selectedItemIndex && useSprayMode[selectedItemIndex];
        bool holdToSpray = useHoldToSpray.Count > selectedItemIndex && useHoldToSpray[selectedItemIndex];
        bool isInfinite = isInfiniteItem.Count > selectedItemIndex && isInfiniteItem[selectedItemIndex];

        bool hasAmmo = isInfinite || (bulletCounts[selectedItemIndex] > 0);
        bool canShoot = Time.time >= lastShotTime[selectedItemIndex] + cooldownTimes[selectedItemIndex];

        bool shouldSprayNow = isSpray
            ? holdToSpray ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)
            : Input.GetMouseButtonDown(0);

        if (shouldSprayNow && hasAmmo && canShoot)
        {
            // Check if this item should only have one instance at a time
            if (spawnOneAtATime.Count > selectedItemIndex && spawnOneAtATime[selectedItemIndex])
            {
                // If a cube of this type is already spawned, destroy it before spawning a new one
                DestroyPreviousCubeOfType(selectedItemIndex);
            }

            // Proceed with spawning the cube or spray
            if (isSpray)
            {
                TrySpraySpawn(selectedItemIndex);
            }
            else
            {
                SpawnCube(selectedItemIndex);
            }

            if (!isInfinite)
                bulletCounts[selectedItemIndex]--;

            lastShotTime[selectedItemIndex] = Time.time;
            UpdateCounterText(selectedItemIndex);
        }

        if (Input.GetMouseButtonUp(0))
        {
            hasSpawnedSpray = false;
        }
    }

    // Destroy previous cube of the same type (Networked)
    private void DestroyPreviousCubeOfType(int indexToSpawn)
    {
        // Check if a cube of this type is already spawned
        if (spawnedCubes.ContainsKey(indexToSpawn))
        {
            GameObject cubeToDestroy = spawnedCubes[indexToSpawn];
            _spawner.Despawn(cubeToDestroy); // Networked despawn
            spawnedCubes.Remove(indexToSpawn); // Remove from dictionary
        }
    }

    private void SpawnCube(int indexToSpawn)
    {
        if (objectToSpawnInFrontOf == null)
        {
            Debug.LogWarning("No object assigned to spawn in front of.");
            return;
        }

        Vector3 spawnPosition = objectToSpawnInFrontOf.transform.position
                               - objectToSpawnInFrontOf.transform.forward * spawnDistance
                               + new Vector3(0, spawnHeight, 0);

        GameObject newCube = _spawner.Spawn(indexToSpawn, spawnPosition,
            objectToSpawnInFrontOf.transform.rotation, Vector3.one);

        // Add the new cube to the dictionary of spawned cubes, indexed by the item type
        spawnedCubes[indexToSpawn] = newCube;

        if (shouldDespawn.Count > indexToSpawn && shouldDespawn[indexToSpawn])
        {
            StartCoroutine(DespawnCubeAfterTime(newCube, despawnTimes[indexToSpawn]));
        }

        Debug.Log($"[Spawner] Spawned cube at {spawnPosition}");
    }

    private void TrySpraySpawn(int indexToSpawn)
    {
        Camera activeCam = sprayCamera != null ? sprayCamera : Camera.main;

        if (activeCam == null)
        {
            Debug.LogError("[Spray] No camera assigned and no main camera found!");
            return;
        }

        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);

        if (drawDebugRaycast)
        {
            Debug.DrawRay(ray.origin, ray.direction * debugRayLength, Color.cyan, 1f);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, debugRayLength))
        {
            Debug.Log($"[Spray] Raycast hit: {hit.collider.name}");

            if (hit.collider.CompareTag("Paintable"))
            {
                Vector3 hitPosition = hit.point + hit.normal * spraySurfaceOffset;

                float distanceToLast = Vector3.Distance(lastSprayPosition, hitPosition);

                if (!hasSpawnedSpray || distanceToLast >= sprayQuadDistance)
                {
                    if (!IsTooCloseToSpray(hitPosition))
                    {
                        GameObject cube = _spawner.Spawn(indexToSpawn, hitPosition, Quaternion.LookRotation(-hit.normal), Vector3.one);

                        if (shouldDespawn.Count > indexToSpawn && shouldDespawn[indexToSpawn])
                        {
                            StartCoroutine(DespawnCubeAfterTime(cube, despawnTimes[indexToSpawn]));
                        }

                        sprayPositions.Add(hitPosition);
                        lastSprayPosition = hitPosition;
                        hasSpawnedSpray = true;

                        Debug.Log($"[Spray] Spawned spray cube at {hitPosition}");
                    }
                    else
                    {
                        Debug.Log("[Spray] Too close to previous spray position. Skipped.");
                    }
                }
                else
                {
                    Debug.Log($"[Spray] Skipped: Only {distanceToLast:F4} units away (needs {sprayQuadDistance}).");
                }
            }
            else
            {
                Debug.Log($"[Spray] Hit object is not tagged as 'Paintable'. Tag is: {hit.collider.tag}");
            }
        }
        else
        {
            Debug.Log("[Spray] Raycast did not hit anything.");
            hasSpawnedSpray = false;
        }
    }

    private bool IsTooCloseToSpray(Vector3 position)
    {
        foreach (Vector3 existing in sprayPositions)
        {
            if (Vector3.Distance(existing, position) < sprayMinDistance)
                return true;
        }
        return false;
    }

    private IEnumerator DespawnCubeAfterTime(GameObject cube, float time)
    {
        yield return new WaitForSeconds(time);
        _spawner.Despawn(cube); // Networked despawn
        spawnedCubes.Remove(cube.GetInstanceID()); // Remove from tracking list
    }

    private void UpdateCounterText(int index)
    {
        if (index >= 0 && index < bulletCounters.Length)
        {
            bulletCounters[index].text = "" + bulletCounts[index];
        }
    }
}
