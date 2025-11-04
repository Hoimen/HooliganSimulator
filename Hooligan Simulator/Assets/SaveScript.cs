using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public Image saveNotificationImage; 
    public Button manualSaveButton; 
    public Button autoSaveButton; 
    public Image autoSaveButtonImage; 

    
    private float saveCooldown = 2f; 
    private bool isSaveOnCooldown = false;

    // Reverting to individual purchase buttons and images
    public Button purchaseButton1;
    public Button purchaseButton2;
    public Button purchaseButton3;
    public Button purchaseButton4;
    public Button purchaseButton5;
    public Button purchaseButton6;
    public Button purchaseButton7;
    public Button purchaseButton8;
    public Button purchaseButton9;
    public Button purchaseButton10;

    public Image purchasedImage1;
    public Image purchasedImage2;
    public Image purchasedImage3;
    public Image purchasedImage4;
    public Image purchasedImage5;
    public Image purchasedImage6;
    public Image purchasedImage7;
    public Image purchasedImage8;
    public Image purchasedImage9;
    public Image purchasedImage10;

    private const string SAVE_KEY = "SavedNumber";
    private const string PURCHASED_KEY = "ItemPurchased"; // Key for boolean save/load
    private int currentNumber = 0;
    private bool isAutoSaveEnabled = true; // Default auto-save is enabled
    private float autoSaveInterval = 60f;
    private float autoSaveTimer = 0f;

    // Panels for UI 
    public GameObject insufficientFundsPanel; 
    public GameObject itemAlreadyPurchasedPanel; 

    private bool[] isItemPurchased = new bool[10]; // 10 items
    private int[] itemCosts = new int[10] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 }; 

   
    public Button resetAllItemsButton;

    
    public float notificationStayDownTime = 0.5f; 

    
    public Transform downPosition; 
    public Transform upPosition;   

    private bool isNotificationDown = false; 

    void Start()
    {
        LoadNumber();
        LoadPurchasedStatus(); 
        DisplayNumber();
        UpdateAutoSaveButtonColor();

       
        if (isAutoSaveEnabled)
        {
            autoSaveTimer = autoSaveInterval; 
        }

        autoSaveButton.onClick.AddListener(ToggleAutoSave);
        manualSaveButton.onClick.AddListener(SaveManual);

        
        purchaseButton1.onClick.AddListener(() => PurchaseItem(0));
        purchaseButton2.onClick.AddListener(() => PurchaseItem(1));
        purchaseButton3.onClick.AddListener(() => PurchaseItem(2));
        purchaseButton4.onClick.AddListener(() => PurchaseItem(3));
        purchaseButton5.onClick.AddListener(() => PurchaseItem(4));
        purchaseButton6.onClick.AddListener(() => PurchaseItem(5));
        purchaseButton7.onClick.AddListener(() => PurchaseItem(6));
        purchaseButton8.onClick.AddListener(() => PurchaseItem(7));
        purchaseButton9.onClick.AddListener(() => PurchaseItem(8));
        purchaseButton10.onClick.AddListener(() => PurchaseItem(9));

       
        purchasedImage1.gameObject.SetActive(isItemPurchased[0]);
        purchasedImage2.gameObject.SetActive(isItemPurchased[1]);
        purchasedImage3.gameObject.SetActive(isItemPurchased[2]);
        purchasedImage4.gameObject.SetActive(isItemPurchased[3]);
        purchasedImage5.gameObject.SetActive(isItemPurchased[4]);
        purchasedImage6.gameObject.SetActive(isItemPurchased[5]);
        purchasedImage7.gameObject.SetActive(isItemPurchased[6]);
        purchasedImage8.gameObject.SetActive(isItemPurchased[7]);
        purchasedImage9.gameObject.SetActive(isItemPurchased[8]);
        purchasedImage10.gameObject.SetActive(isItemPurchased[9]);

       
        resetAllItemsButton.onClick.AddListener(ResetAllItems);
    }

    void Update()
    {
        if (isAutoSaveEnabled)
        {
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer >= autoSaveInterval)
            {
                SaveNumber(currentNumber);
                autoSaveTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.N))  
        {
            currentNumber++;
            DisplayNumber();
        }
    }

    public void SaveNumber(int numberToSave)
    {
        PlayerPrefs.SetInt(SAVE_KEY, numberToSave);
        PlayerPrefs.Save();
        Debug.Log("Game Saved: " + numberToSave);

        // Trigger save notification animation
        StartCoroutine(AnimateSaveNotification());
    }

    IEnumerator AnimateSaveNotification()
    {
        if (isNotificationDown) yield break; 

       
        Vector3 startPos = saveNotificationImage.transform.position;
        Vector3 endPos = downPosition.position; 
        float duration = 0.5f; 

        float startTime = Time.time;
        float elapsedTime = 0f;

       
        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            saveNotificationImage.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

       
        isNotificationDown = true;

       
        yield return new WaitForSeconds(notificationStayDownTime);

        
        Vector3 targetPos = upPosition.position;

        startTime = Time.time;
        elapsedTime = 0f;

       
        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            saveNotificationImage.transform.position = Vector3.Lerp(endPos, targetPos, t);
            yield return null;
        }

        
        saveNotificationImage.transform.position = targetPos;

        
        isNotificationDown = false;
    }

    public void SaveManual()
    {
        if (isSaveOnCooldown)
        {
            Debug.Log("Save is on cooldown.");
            return;
        }

        SaveNumber(currentNumber);
        StartCoroutine(CooldownSaveButton());
    }

    IEnumerator CooldownSaveButton()
    {
        isSaveOnCooldown = true;
        manualSaveButton.interactable = false; 
        yield return new WaitForSeconds(saveCooldown); 
        isSaveOnCooldown = false;
        manualSaveButton.interactable = true; 
    }

    public void LoadNumber()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            currentNumber = PlayerPrefs.GetInt(SAVE_KEY);
            Debug.Log("Loaded Number: " + currentNumber);
        }
        else
        {
            Debug.Log("No saved data found.");
        }
    }

    void DisplayNumber()
    {
        numberText.text = "$ " + currentNumber.ToString();
    }

    public void ToggleAutoSave()
    {
        isAutoSaveEnabled = !isAutoSaveEnabled;
        string status = isAutoSaveEnabled ? "Enabled" : "Disabled";
        Debug.Log("Auto-Save " + status);

        UpdateAutoSaveButtonColor();
    }

    void UpdateAutoSaveButtonColor()
    {
        autoSaveButtonImage.color = isAutoSaveEnabled ? Color.green : Color.red;
    }

    public void PurchaseItem(int index)
    {
        if (index >= 0 && index < 10)
        {
            if (!isItemPurchased[index] && currentNumber >= itemCosts[index])
            {
                currentNumber -= itemCosts[index]; 
                isItemPurchased[index] = true; 

               
                switch (index)
                {
                    case 0: purchasedImage1.gameObject.SetActive(true); break;
                    case 1: purchasedImage2.gameObject.SetActive(true); break;
                    case 2: purchasedImage3.gameObject.SetActive(true); break;
                    case 3: purchasedImage4.gameObject.SetActive(true); break;
                    case 4: purchasedImage5.gameObject.SetActive(true); break;
                    case 5: purchasedImage6.gameObject.SetActive(true); break;
                    case 6: purchasedImage7.gameObject.SetActive(true); break;
                    case 7: purchasedImage8.gameObject.SetActive(true); break;
                    case 8: purchasedImage9.gameObject.SetActive(true); break;
                    case 9: purchasedImage10.gameObject.SetActive(true); break;
                }

                SaveNumber(currentNumber); 
                SavePurchasedStatus(); 
                DisplayNumber(); 
                Debug.Log("Item " + (index + 1) + " purchased! Money deducted: $" + itemCosts[index]);
            }
            else if (isItemPurchased[index])
            {
                Debug.Log("Item " + (index + 1) + " is already purchased.");
                itemAlreadyPurchasedPanel.SetActive(true); 
            }
            else
            {
                Debug.Log("Not enough money to purchase Item " + (index + 1));
                insufficientFundsPanel.SetActive(true); 
            }
        }
    }

    void SavePurchasedStatus()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt(PURCHASED_KEY + i, isItemPurchased[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
        Debug.Log("Purchased Status Saved");
    }

    void LoadPurchasedStatus()
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey(PURCHASED_KEY + i))
            {
                isItemPurchased[i] = PlayerPrefs.GetInt(PURCHASED_KEY + i) == 1;
                Debug.Log("Loaded Purchased Status for Item " + (i + 1) + ": " + isItemPurchased[i]);
            }
        }
    }

    
    public void ResetAllItems()
    {
        for (int i = 0; i < 10; i++)
        {
            isItemPurchased[i] = false;
            switch (i)
            {
                case 0: purchasedImage1.gameObject.SetActive(false); break;
                case 1: purchasedImage2.gameObject.SetActive(false); break;
                case 2: purchasedImage3.gameObject.SetActive(false); break;
                case 3: purchasedImage4.gameObject.SetActive(false); break;
                case 4: purchasedImage5.gameObject.SetActive(false); break;
                case 5: purchasedImage6.gameObject.SetActive(false); break;
                case 6: purchasedImage7.gameObject.SetActive(false); break;
                case 7: purchasedImage8.gameObject.SetActive(false); break;
                case 8: purchasedImage9.gameObject.SetActive(false); break;
                case 9: purchasedImage10.gameObject.SetActive(false); break;
            }
        }

        SavePurchasedStatus(); // Save the reset status
        Debug.Log("All items have been reset.");
    }
}
