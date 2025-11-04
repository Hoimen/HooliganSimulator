using UnityEngine;
using Alteruna;

public class SyncedObjectHider : Synchronizable
{
    [SerializeField] private GameObject objectToHide; 
    [SerializeField] private KeyCode hideKey = KeyCode.K; // i think K is already used but whatever lol

    private bool isObjectVisible = true; 

    private void Update()
    {
        if (Input.GetKeyDown(hideKey))
        {
            isObjectVisible = !isObjectVisible; // Toggle visibility
            ToggleObjectVisibility(isObjectVisible);
            SyncVisibility(isObjectVisible);
        }
    }

    private void ToggleObjectVisibility(bool isVisible)
    {
        objectToHide.SetActive(isVisible);
    }

    private void SyncVisibility(bool isVisible)
    {
        Commit(); // Synchronize across network (FIX THIS)
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        isObjectVisible = reader.ReadBool();
        ToggleObjectVisibility(isObjectVisible);
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        writer.Write(isObjectVisible);
    }
}
