using UnityEngine;
using Alteruna;

public class ShowObjectSynchronizable3 : Synchronizable
{
    public GameObject objectToShow; 
    public bool isObjectVisible = false; // Visibility state 'boolean'
    private bool _previousVisibilityState;

    
    private Alteruna.Avatar _avatar;

    // Editable settings do this more
    [SerializeField]
    private int updatesPerSecond = 10; 

    private float _updateInterval;
    private float _timeSinceLastUpdate;

    private void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
        _updateInterval = 1f / updatesPerSecond; 
        UpdateObjectVisibility(); 
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        isObjectVisible = reader.ReadBool();
        UpdateObjectVisibility();
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        writer.Write(isObjectVisible);
    }

    private void Update()
    {
        if (!_avatar.IsMe) return; 

        // F kea change later 
        if (Input.GetKeyDown(KeyCode.F))
        {
            isObjectVisible = !isObjectVisible;
            Commit(); 
            UpdateObjectVisibility();
        }

        
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= _updateInterval)
        {
            _timeSinceLastUpdate = 0f;

            
            if (_previousVisibilityState != isObjectVisible)
            {
                Commit(); 
                _previousVisibilityState = isObjectVisible;
            }
        }

        base.SyncUpdate(); // Call the Alteruna sync update
    }

    private void UpdateObjectVisibility()
    {
        if (objectToShow != null)
        {
            objectToShow.SetActive(isObjectVisible);
        }
    }
}


