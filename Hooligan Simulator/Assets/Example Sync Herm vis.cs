using Alteruna;
using UnityEngine;

public class ShowObjectsSynchronizable : Synchronizable //my example (also not very good)
{
    [Header("Assign exactly 10 GameObjects in the inspector")]
    public GameObject[] objectsToShow = new GameObject[20];

    
    public bool[] isObjectsVisible = new bool[20];
    private bool[] _previousVisibilityStates = new bool[20];

   
    private Alteruna.Avatar _avatar;

    [SerializeField]
    private int updatesPerSecond = 10;        

    private float _updateInterval;
    private float _timeSinceLastUpdate;

    private void Start()

    {
        _avatar = GetComponent<Alteruna.Avatar>();
        _updateInterval = 1f / updatesPerSecond;

        
        int len = Mathf.Min(objectsToShow.Length, isObjectsVisible.Length);
        _previousVisibilityStates = new bool[len];
        for (int i = 0; i < len; i++)
        {
            bool initial = objectsToShow[i] != null && objectsToShow[i].activeSelf;
            isObjectsVisible[i] = initial;
            _previousVisibilityStates[i] = initial;
        }

        UpdateObjectVisibility();
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        
        int len = Mathf.Min(objectsToShow.Length, isObjectsVisible.Length);
        for (int i = 0; i < len; i++)
        {
            isObjectsVisible[i] = reader.ReadBool();
            _previousVisibilityStates[i] = isObjectsVisible[i];
        }
        UpdateObjectVisibility();
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        
        int len = Mathf.Min(objectsToShow.Length, isObjectsVisible.Length);
        for (int i = 0; i < len; i++)
        {
            writer.Write(isObjectsVisible[i]);
        }
    }

    private void Update()
    {
        if (!_avatar.IsMe) return;  // Only local owner sends updates

        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= _updateInterval)
        {
            _timeSinceLastUpdate = 0f;

            bool anyChange = false;
            int len = Mathf.Min(objectsToShow.Length, _previousVisibilityStates.Length);
            for (int i = 0; i < len; i++)
            {
                bool curr = objectsToShow[i] != null && objectsToShow[i].activeSelf;
                if (curr != _previousVisibilityStates[i])
                {
                    // Local toggle detected
                    isObjectsVisible[i] = curr;
                    _previousVisibilityStates[i] = curr;
                    anyChange = true;
                }
            }

            if (anyChange)
            {
                Commit();            // Sync out all 10 states
                UpdateObjectVisibility();
            }
        }

        base.SyncUpdate(); // Let Alteruna do its normal per-frame sync work
    }

    private void UpdateObjectVisibility()
    {
        int len = Mathf.Min(objectsToShow.Length, isObjectsVisible.Length);
        for (int i = 0; i < len; i++)
        {
            if (objectsToShow[i] != null)
                objectsToShow[i].SetActive(isObjectsVisible[i]);
        }
    }
}
