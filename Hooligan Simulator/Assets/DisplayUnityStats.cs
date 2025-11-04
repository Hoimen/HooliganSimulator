using UnityEngine;
using TMPro;
using System.Net.NetworkInformation;
using System.Collections;

public class DisplayUnityStats : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    private int pingValue = -1;

    void Start()
    {
        if (statsText == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
            enabled = false;
            return;
        }

        Application.targetFrameRate = -1; // Unlock frame rate

        StartCoroutine(CheckPing());
    }

    void Update()
    {
        UpdateStatsText();
    }

    void UpdateStatsText()
    {
        statsText.text = "";

        // FPS
        statsText.text += "FPS: " + Mathf.RoundToInt(1f / Time.deltaTime) + "\n";

        // Resolution
        statsText.text += "Resolution: " + Screen.width + "x" + Screen.height + "\n";

        // Connection
        statsText.text += "Internet: " + (HasInternetConnection() ? "Connected" : "Disconnected") + "\n";

        // Ping (Network Latency)
        statsText.text += "Ping: " + (pingValue >= 0 ? pingValue + " ms" : "N/A") + "\n";

        // Device Model & OS because why not
        statsText.text += "Device Model: " + SystemInfo.deviceModel + "\n";
        statsText.text += "OS: " + SystemInfo.operatingSystem + "\n";

        statsText.ForceMeshUpdate();
    }

    bool HasInternetConnection()
    {
        try
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
        catch
        {
            return false;
        }
    }

    IEnumerator CheckPing()
    {
        while (true)
        {
            UnityEngine.Ping ping = new UnityEngine.Ping("8.8.8.8"); 
            yield return new WaitUntil(() => ping.isDone);

            pingValue = ping.time; // Get the ping result (in milliseconds)
            yield return new WaitForSeconds(5); // can change update speed if neeeded 
        }
    }
}
