using UnityEngine;
using UnityEngine.UI;  // Chatgpt slop
using TMPro;
using Alteruna;

public class ChatDisplayAboveHead : MonoBehaviour
{
    public TextMeshProUGUI textMesh; // TextMeshProUGUI for UI-based text
    private TextChatSynchronizable textChatSynchronizable;

    public Canvas canvas; // Reference to the canvas containing the text
    public Vector3 offset = new Vector3(0, 2, 0); // Offset to adjust the height above the player's head

    public InputField inputField; // Public InputField to assign in the Unity Inspector
    private string latestMessage = ""; // Store the player's latest message

    private void Start()
    {
        // Ensure TextChatSynchronizable component exists
        textChatSynchronizable = FindObjectOfType<TextChatSynchronizable>();
        if (textChatSynchronizable == null)
        {
            Debug.LogError("TextChatSynchronizable not found in the scene.");
            return;
        }

        // Ensure TextMeshProUGUI is assigned
        if (textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned.");
            return;
        }

        // Ensure InputField is assigned in the Inspector
        if (inputField == null)
        {
            Debug.LogWarning("InputField is not assigned in the Inspector.");
        }

        // Add listener for when the player submits a chat message (via InputField)
        inputField.onEndEdit.AddListener(OnPlayerSendMessage);

        // Subscribe to the TextChatUpdate event
        textChatSynchronizable.TextChatUpdate.AddListener(OnChatUpdated);
    }

    private void Update()
    {
        // Update the position of the text above the player's head in world space
        Vector3 worldPos = transform.position + offset; // Adjust offset if needed
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos); // Convert world position to screen position

        // If canvas is WorldSpace, set the world position of the text
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            textMesh.transform.position = worldPos;
        }
        else
        {
            // If canvas is Screen Space, use screen position
            textMesh.transform.position = screenPos;
        }
    }

    private void OnDestroy()
    {
        // Remove listeners when the object is destroyed
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnPlayerSendMessage);
        }

        if (textChatSynchronizable != null)
        {
            textChatSynchronizable.TextChatUpdate.RemoveListener(OnChatUpdated);
        }
    }

    private void OnChatUpdated(string latestChatMessage)
    {
        // Only update if the incoming message is the player's latest message
        if (latestChatMessage == latestMessage)
        {
            textMesh.text = latestMessage;
        }
    }

    private void OnPlayerSendMessage(string message)
    {
        // Log the raw message when Enter is pressed
        Debug.Log("Raw message from InputField (onEndEdit): '" + message + "'");

        // Trim the input to remove leading/trailing spaces and check the result
        message = message.Trim();

        // Log the trimmed message
        Debug.Log("Trimmed message: '" + message + "'");

        // Check if the trimmed message contains any valid characters
        if (!string.IsNullOrEmpty(message))
        {
            // Store the latest message typed by the player
            latestMessage = message;

            // Update the TextMeshProUGUI with the player's latest message
            textMesh.text = latestMessage;
            Debug.Log("TextMeshProUGUI updated with message: " + latestMessage);
        }
        else
        {
            // Log more information if the message is considered empty
            Debug.LogWarning("Player submitted an empty or whitespace-only message.");
        }
    }
}
