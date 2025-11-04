using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Alteruna;

namespace Alteruna_Examples  //Entirly usless script thanks alot chatgpt
{
    /// <summary>
    /// Class <c>ExampleTextMeshProSync</c> demonstrates synchronizing TextMeshPro input text using Alteruna.
    /// </summary>
    public class ExampleTextMeshProSync : Synchronizable
    {
        // TextMeshPro component to display the synchronized text
        public TextMeshProUGUI synchronizedText;

        // TMP_InputField for player input
        public TMP_InputField inputField;

        // The string that we want to synchronize
        public string synchronizedString = "Player"; // Default value "Player"

        // To keep track of the previous string to check for changes
        private string _oldSynchronizedString = "";

        private Alteruna.Avatar _avatar; // Reference to the Avatar component to identify the local player

        private void Start()
        {
            _avatar = GetComponent<Alteruna.Avatar>();

            // Ensure only the local player can edit their text
            if (_avatar.IsMe)
            {
                // Initialize synchronizedText if not set in the Inspector
                if (synchronizedText == null)
                    synchronizedText = GetComponent<TextMeshProUGUI>();

                // Set the default text to "Player"
                synchronizedText.text = synchronizedString;

                // Initialize inputField if not set in the Inspector
                if (inputField != null)
                {
                    inputField.onValueChanged.AddListener(OnInputFieldChanged);
                    inputField.interactable = true; // Enable input field for local player
                }
            }
            else
            {
                // Disable the input field for non-local players
                if (inputField != null)
                {
                    inputField.interactable = false;
                }
            }
        }

        private void OnInputFieldChanged(string newText)
        {
            // If this player is the local player, update the synchronizedString
            if (_avatar.IsMe)
            {
                synchronizedString = newText;
                // Immediately update the displayed text as well
                synchronizedText.text = synchronizedString;
            }
        }

        public override void DisassembleData(Reader reader, byte LOD)
        {
            // Get the synchronized text from other players
            synchronizedString = reader.ReadString();

            // Update the text component to reflect the synchronized value
            synchronizedText.text = synchronizedString;

            // Save the new data as the old data to avoid unnecessary changes
            _oldSynchronizedString = synchronizedString;
        }

        public override void AssembleData(Writer writer, byte LOD)
        {
            // Write the synchronized string so that it can be sent to other players
            writer.Write(synchronizedString);
        }

        private void Update()
        {
            // If the value of our string has changed, sync it with the other players
            if (synchronizedString != _oldSynchronizedString)
            {
                // Store the updated value
                _oldSynchronizedString = synchronizedString;

                // Tell Alteruna that we want to commit our data
                Commit();
            }

            // Update the Synchronizable (to sync the data)
            SyncUpdate();
        }
    }
}
