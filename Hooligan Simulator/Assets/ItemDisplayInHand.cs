using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    public GameObject[] gunDisplays; // stole this from my old game lol

    public void UpdateGunDisplay(int selectedSlot, int[] itemIndicesInSlots)
    {
        HideAllGunDisplays();

        if (selectedSlot != -1 && itemIndicesInSlots[selectedSlot] != -1)
        {
            gunDisplays[itemIndicesInSlots[selectedSlot]].SetActive(true);
        }
    }

    private void HideAllGunDisplays()
    {
        foreach (var gunDisplay in gunDisplays)
        {
            gunDisplay.SetActive(false);
        }
    }
}
