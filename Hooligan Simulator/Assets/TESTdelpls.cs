using UnityEngine;
using UnityEngine.UI;
using Alteruna;

public class ShowClothingSynchronizable : Synchronizable
{
    // Headwear
    public GameObject[] headwear;
    public GameObject[] fakeHeadwear;
    public Button[] headwearToggleButtons;
    public Button headwearUnequipAllButton;

    // Body types
    public GameObject[] bodyTypes;
    public GameObject[] fakeBodyTypes;
    public Button[] bodyTypeToggleButtons;
    public Button bodyTypeUnequipAllButton;

    // Hair
    public GameObject[] hair;
    public GameObject[] fakeHair;
    public Button[] hairToggleButtons;
    public Button hairUnequipAllButton;

    // Faces
    public GameObject[] faces;
    public GameObject[] fakeFaces;
    public Button[] faceToggleButtons;
    public Button faceUnequipAllButton;

   
    [SerializeField]
    private GameObject playerObject; 
    [SerializeField]
    private GameObject visibilityIndicator; 
    [SerializeField]
    private GameObject person; 

    private Alteruna.Avatar _avatar;

    [SerializeField]
    private int updatesPerSecond = 10;

    private float _updateInterval;
    private float _timeSinceLastUpdate;

    private void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
        _updateInterval = 1f / updatesPerSecond;

        InitializeToggleButtons(headwear, headwearToggleButtons, ToggleHeadwear, headwearUnequipAllButton, UnequipAllHeadwear);
        InitializeToggleButtons(bodyTypes, bodyTypeToggleButtons, ToggleBodyType, bodyTypeUnequipAllButton, UnequipAllBodyTypes);
        InitializeToggleButtons(hair, hairToggleButtons, ToggleHair, hairUnequipAllButton, UnequipAllHair);
        InitializeToggleButtons(faces, faceToggleButtons, ToggleFace, faceUnequipAllButton, UnequipAllFaces);
    }

    private void Update()
    {
        if (!_avatar.IsMe) return;

        
        if (visibilityIndicator.activeSelf)
        {
            person.SetActive(false);
        }
        else
        {
            person.SetActive(true);
        }

        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= _updateInterval)
        {
            _timeSinceLastUpdate = 0f;
            Commit();
        }

        base.SyncUpdate();
    }

    private void InitializeToggleButtons(GameObject[] items, Button[] buttons, System.Action<int> toggleAction, Button unequipAllButton, System.Action unequipAllAction)
    {
        if (items.Length != buttons.Length)
        {
            Debug.LogError("Items and buttons arrays must have the same length.");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => toggleAction(index));
        }

        if (unequipAllButton != null)
        {
            unequipAllButton.onClick.AddListener(() => unequipAllAction());
        }
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        UpdateVisibilityFromData(reader, headwear);
        UpdateVisibilityFromData(reader, bodyTypes);
        UpdateVisibilityFromData(reader, hair);
        UpdateVisibilityFromData(reader, faces);
    }

    private void UpdateVisibilityFromData(Reader reader, GameObject[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            bool isVisible = reader.ReadBool();
            items[i].SetActive(isVisible);
        }
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        WriteVisibilityToData(writer, headwear);
        WriteVisibilityToData(writer, bodyTypes);
        WriteVisibilityToData(writer, hair);
        WriteVisibilityToData(writer, faces);
    }

    private void WriteVisibilityToData(Writer writer, GameObject[] items)
    {
        foreach (var item in items)
        {
            writer.Write(item.activeSelf);
        }
    }

    private void ToggleHeadwear(int index)
    {
        ToggleItem(headwear, fakeHeadwear, index);
        Commit();
    }

    private void UnequipAllHeadwear()
    {
        UnequipAllItems(headwear, fakeHeadwear);
        Commit();
    }

    private void ToggleBodyType(int index)
    {
        ToggleItem(bodyTypes, fakeBodyTypes, index);
        Commit();
    }

    private void UnequipAllBodyTypes()
    {
        UnequipAllItems(bodyTypes, fakeBodyTypes);
        Commit();
    }

    private void ToggleHair(int index)
    {
        ToggleItem(hair, fakeHair, index);
        Commit();
    }

    private void UnequipAllHair()
    {
        UnequipAllItems(hair, fakeHair);
        Commit();
    }

    private void ToggleFace(int index)
    {
        ToggleItem(faces, fakeFaces, index);
        Commit();
    }

    private void UnequipAllFaces()
    {
        UnequipAllItems(faces, fakeFaces);
        Commit();
    }

    private void ToggleItem(GameObject[] realItems, GameObject[] fakeItems, int index)
    {
        for (int i = 0; i < realItems.Length; i++)
        {
            bool isActive = i == index;
            realItems[i].SetActive(isActive);
            fakeItems[i]?.SetActive(isActive);
        }
    }

    private void UnequipAllItems(GameObject[] realItems, GameObject[] fakeItems)
    {
        foreach (var item in realItems)
        {
            item.SetActive(false);
        }

        foreach (var item in fakeItems)
        {
            item?.SetActive(false);
        }
    }
}
