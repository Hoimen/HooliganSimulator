using UnityEngine;
using Alteruna;

public class SetName : CommunicationBridge
{
    public string playerName; // change later if needed

    private void Awake()
    {
        base.OnEnable();

        // Array of possible player names
        string[] nameOptions = new string[]
        {
            "Player#100", "Player#101", "Player#102", "Player#103",
            "Player#104", "Player#105", "Player#106", "Player#107",
            "Player#108", "Player#109", "Player#110", "Player#112",
            "Player#113", "Player#114", "Player#115", "Player#116",
            "Player#117", "Player#118", "Player#119", "Player#120"
        };

        // Assign a random name from the array
        playerName = nameOptions[Random.Range(0, nameOptions.Length)];

        // Set the name in Alteruna's multiplayer system
        Multiplayer.SetUsername(playerName);
    }
}
