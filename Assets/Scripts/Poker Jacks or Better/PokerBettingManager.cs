using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Included for TextMeshPro
using UnityEngine.UI;

public class PokerBettingManager : MonoBehaviour
{
    public int currentBet = 0;// tracks current bet amount 1-5
    public GameObject[] betPanels; // Array of bet panels
    public TMP_Text betText; // UI Text that shows the current bet
    public Button betOneButton; // Bet 1 button
    public Button betFiveButton; // Bet 5 button
    public Button dealButton; // Deal button
    public Button drawButton; // Draw button, initially not active
    public Sprite inactiveSprite; // Sprite for inactive bet panel
    public Sprite activeSprite; // Sprite for active bet panel

    public AmountManager amountManager;// Reference to the AmountManager script
    public PokerLogicManager pokerLogicManager; // Reference to the PokerLogicManager script

    void Start()
    {
        drawButton.gameObject.SetActive(false); // Ensure the draw button is not active at the start
        UpdateBetDisplay();
    }

    public void OnBetOnePressed()//cycles between current bet(starting at 1 if this is the first button press, or second button pressed after bet 5 has been pressed)
    {
        currentBet = (currentBet % 5) + 1;
        HighlightBetPanel(currentBet - 1);
        UpdateBetDisplay();
    }

    public void OnBetFivePressed()//goes straight to max bet(5)
    {
        currentBet = 5;
        HighlightBetPanel(currentBet - 1);
        UpdateBetDisplay();
    }

    private void HighlightBetPanel(int panelIndex)//highlights the current reward column based on currentBet
    {
        // Logic to switch images
        foreach (GameObject panel in betPanels)
        {
            // Set all panels to inactive sprite
            panel.GetComponent<Image>().sprite = inactiveSprite;
        }
        // Set the active panel to active sprite
        betPanels[panelIndex].GetComponent<Image>().sprite = activeSprite;
    }

    private void UpdateBetDisplay()
    {
        betText.text = "$" + currentBet;
    }

    public void OnDealPressed()
    {
        if (currentBet > 0)
        {
            AmountManager.Instance.AddAmount(-currentBet); // Deduct the current bet from the total amount
            betOneButton.interactable = false;
            betFiveButton.interactable = false;
            dealButton.gameObject.SetActive(false); // Deactivate the deal button
            drawButton.gameObject.SetActive(true); // Activate the draw button
            pokerLogicManager.ResetForNewHand(); // Prepares the game for a new hand
            pokerLogicManager.DealInitialHand(); // Start dealing the hand
        }
        else
        {
            Debug.LogWarning("Attempted to deal with a bet of 0.");
        }
    }

    /*public void ResetForNewGame()
    {
        betOneButton.interactable = true;
        betFiveButton.interactable = true;
        dealButton.gameObject.SetActive(true); // Reactivate the deal button
        drawButton.gameObject.SetActive(false); // Deactivate the draw button
        currentBet = 0; // Reset the bet amount
        UpdateBetDisplay();
    }*/ //For testing UI pre- Logic Script
}