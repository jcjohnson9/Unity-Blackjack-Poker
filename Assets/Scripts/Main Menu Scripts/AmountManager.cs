using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include for UI elements
using TMPro; // Included for TextMeshPro
using UnityEngine.SceneManagement; // For managing scenes

public class AmountManager : MonoBehaviour
{
    public static AmountManager Instance; // Singleton pattern

    public TMP_Text totalAmountText;//the text for how much total money the player has
    public int totalAmount = 0;//How much total money the player has

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//carries amount manager between scenes to save its value
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);//destroys duplicates
        }
        Debug.Log("AmountManager Awake");
        // Existing Awake implementation...
    }

    void Start()
    {
        UpdateAmountDisplay();
    }

    public void AddAmount(int amount)
    {
        totalAmount += amount;
        UpdateAmountDisplay();
    }

    public void SubtractAmount(int amount)
    {
        totalAmount -= amount;
        UpdateAmountDisplay();
    }

    void UpdateAmountDisplay()
    {
        if (totalAmountText != null) // Ensure totalAmountText is not null
        {
            totalAmountText.text = "Total: $" + totalAmount.ToString();
        }
    }

    public void ResetTotal()// currently unused
    {
        totalAmount = 0;
        UpdateAmountDisplay();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject textObject = GameObject.FindGameObjectWithTag("TotalAmountText");//find total amount text in scene
        if (textObject != null)
        {
            totalAmountText = textObject.GetComponent<TMP_Text>();
            UpdateAmountDisplay();
        }
        else
        {
            Debug.LogWarning("TotalAmountText object not found in the scene.");
        }
        Debug.Log($"Loaded scene: {scene.name}");
        // Existing OnSceneLoaded implementation...
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}