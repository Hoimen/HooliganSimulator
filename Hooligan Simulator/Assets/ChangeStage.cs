using UnityEngine;
using Alteruna;

public class ShowObjectSynchronizable2 : Synchronizable
{
    public GameObject objectToShow; 
    private bool isObjectVisible = false; 
    private bool _previousVisibilityState;

    
    [SerializeField]
    private int updatesPerSecond = 10; 
    private float _updateInterval;
    private float _timeSinceLastUpdate;

    private void Start()
    {
        _updateInterval = 1f / updatesPerSecond; // Calculate update interval
        UpdateObjectVisibility(); 
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        
        isObjectVisible = reader.ReadBool();
        UpdateObjectVisibility();
        _previousVisibilityState = isObjectVisible; 
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        
        writer.Write(isObjectVisible);
    }

    private void Update()
    {
        // Toggle visibility with the "F" key (for local player)
        if (Input.GetKeyDown(KeyCode.F))
        {
            isObjectVisible = !isObjectVisible;
            UpdateObjectVisibility();
            Commit(); 
        }

        // Throttle updates to reduce network traffic
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= _updateInterval)
        {
            _timeSinceLastUpdate = 0f;

            // Ensure synchronization is correct
            if (_previousVisibilityState != isObjectVisible)
            {
                Commit(); // Sync state
                _previousVisibilityState = isObjectVisible; // Update tracking
            }
        }

        base.SyncUpdate(); // Ensure Alteruna's internal sync updates properly
    }

    private void UpdateObjectVisibility()
    {
        if (objectToShow != null)
        {
            objectToShow.SetActive(isObjectVisible);
        }
    }
}
