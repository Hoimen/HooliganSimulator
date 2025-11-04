using UnityEngine;
using UnityEngine.UI;
using SFB; // This is the namespace for Unity Standalone File Browser
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class MP3FileSelector : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component to play audio

    // This method will be called when the player clicks the "Select MP3" button
    public void OpenFileDialog()
    {
        // Open the file picker dialog, allowing the user to choose an MP3 file
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an MP3", "", "mp3", false);

        // If a file is selected
        if (paths.Length > 0)
        {
            string filePath = paths[0]; // Get the path of the selected file

            // Check if the file has the ".mp3" extension
            if (IsMP3File(filePath))
            {
                PlayMP3(filePath); // If it's an MP3, play it
            }
            else
            {
                Debug.LogError("Selected file is not an MP3.");
            }
        }
    }

    // Check if the file has a ".mp3" extension
    private bool IsMP3File(string filePath)
    {
        return Path.GetExtension(filePath).ToLower() == ".mp3";
    }

    // Play the MP3 file using UnityWebRequest
    private void PlayMP3(string filePath)
    {
        StartCoroutine(LoadMP3AndPlay(filePath));
    }

    // Load and play the MP3 asynchronously
    private IEnumerator LoadMP3AndPlay(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            // If the file loads successfully
            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip; // Assign the audio clip to the AudioSource
                audioSource.Play(); // Play the audio
            }
            else
            {
                Debug.LogError("Failed to load MP3: " + www.error);
            }
        }
    }
}
