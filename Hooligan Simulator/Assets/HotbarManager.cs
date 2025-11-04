using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots; // 4 slots
    public GameObject[] items; // 4 items
    public GameObject[] gunDisplays; // 4 corresponding 3D display prefabs
    public Image[] slotBorders; // UI images
    public GameObject deleteConfirmationPanel; 
    

    public Button[] slotButtons; //slot buttons (1-4)
    public Button[] itemSpawnButtons; 

    private int[] itemIndicesInSlots = new int[4]; // item held tracker (-1 means empty btw)
    private int selectedSlot = 0; // Keeps track of currently selected slot (with 0 being frist)
    private bool isDeletePanelVisible = false; // Tracks if the delete confirmation panel is visible
    private Alteruna.Avatar _avatar; // multiplayer stuffs

    private void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        // Only initialize if this is the local player
        if (!_avatar.IsMe)
            return;

       
        for (int i = 0; i < itemIndicesInSlots.Length; i++)
        {
            itemIndicesInSlots[i] = -1;
        }

        // Spawn the starting item
       // if (startingItem >= 0 && startingItem < items.Length) HA I REMOVED IT, i guess no starting item )=
        //{
         //   SpawnItem(startingItem);
        //}

        
        HideAllGunDisplays();

       
        SelectSlot(0);

        // hide delete pannel at start
        deleteConfirmationPanel.SetActive(false);

       
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i; 
            slotButtons[i].onClick.AddListener(() => SelectSlot(index));
        }

        
        for (int i = 0; i < itemSpawnButtons.Length; i++)
        {
            int itemIndex = i; 
            itemSpawnButtons[i].onClick.AddListener(() => SpawnItem(itemIndex));
        }
    }

    private void Update()
    {
        
        if (!_avatar.IsMe)
            return;

        //  1-4 keys being pressed to select slots
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // 1-4 keys
            {
                SelectSlot(i);
                break;
            }
        }

        // Q for delete current item
        if (Input.GetKeyDown(KeyCode.Q) && selectedSlot != -1 && itemIndicesInSlots[selectedSlot] != -1)
        {
            ShowDeleteConfirmation();
        }

        
        if (isDeletePanelVisible)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Confirm deletion with Q
            {
                DeleteSelectedItem();
            }
            else if (Input.anyKeyDown) // Any other key to cancel
            {
                HideDeleteConfirmation();
            }
        }
    }

    private void ShowDeleteConfirmation()
    {
        deleteConfirmationPanel.SetActive(true);
        isDeletePanelVisible = true;
    }

    private void HideDeleteConfirmation()
    {
        deleteConfirmationPanel.SetActive(false);
        isDeletePanelVisible = false;
    }

    private void DeleteSelectedItem()
    {
        if (selectedSlot != -1 && itemIndicesInSlots[selectedSlot] != -1)
        {
            int itemIndex = itemIndicesInSlots[selectedSlot];
            Destroy(inventorySlots[selectedSlot].transform.GetChild(0).gameObject);
            gunDisplays[itemIndex].SetActive(false);
            itemIndicesInSlots[selectedSlot] = -1;
            HideDeleteConfirmation();
            Debug.Log("Item deleted from slot " + (selectedSlot + 1));
        }
    }

    private void DeselectAllSlots()
    {
        for (int i = 0; i < slotBorders.Length; i++)
        {
            slotBorders[i].enabled = false;
        }
        selectedSlot = -1;
        HideAllGunDisplays();
        Debug.Log("All slots deselected.");
    }

    private void HideAllGunDisplays()
    {
        foreach (var gunDisplay in gunDisplays)
        {
            gunDisplay.SetActive(false);
        }
    }

    private void SpawnItem(int itemIndex)
    {
        if (!_avatar.IsMe)
            return;

        for (int i = 0; i < itemIndicesInSlots.Length; i++)
        {
            if (itemIndicesInSlots[i] == itemIndex)
            {
                Debug.Log($"Item {itemIndex + 1} is already in slot {i + 1}.");
                return;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount == 0)
            {
                GameObject spawnedItem = Instantiate(items[itemIndex], inventorySlots[i].transform);
                spawnedItem.transform.localPosition = Vector3.zero;
                itemIndicesInSlots[i] = itemIndex;

                if (i == selectedSlot)
                {
                    gunDisplays[itemIndex].SetActive(true);
                }

                return;
            }
        }

        Debug.Log("All slots are filled.");
    }

    private void SelectSlot(int slotIndex)
    {
        if (!_avatar.IsMe)
            return;

        if (slotIndex < 0 || slotIndex >= slotBorders.Length)
        {
            Debug.LogError("Slot index out of range.");
            return;
        }

        for (int i = 0; i < slotBorders.Length; i++)
        {
            slotBorders[i].enabled = false;

            if (i != slotIndex && itemIndicesInSlots[i] != -1)
            {
                gunDisplays[itemIndicesInSlots[i]].SetActive(false);
            }
        }

        slotBorders[slotIndex].enabled = true;
        selectedSlot = slotIndex;

        int itemIndex = itemIndicesInSlots[slotIndex];
        if (itemIndex != -1)
        {
            gunDisplays[itemIndex].SetActive(true);
            Debug.Log($"Slot {slotIndex + 1} selected. Showing gun display for item {itemIndex + 1}.");
        }
        else
        {
            Debug.Log($"Slot {slotIndex + 1} selected, but no item is present.");
        }
    }

    public int GetSelectedItemIndex()
    {
        if (!_avatar.IsMe)
            return -1;

        return selectedSlot != -1 ? itemIndicesInSlots[selectedSlot] : -1;
    }
}