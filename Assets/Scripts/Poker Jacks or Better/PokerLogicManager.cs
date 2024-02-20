using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Included for TextMeshPro
using UnityEngine.UI;

public class PokerLogicManager : MonoBehaviour
{
    public DeckManager deckManager;
    public PokerBettingManager pokerBettingManager;

    [SerializeField] private GameObject cardButtonPrefab;
    public List<Transform> cardPositions; // Manually assign these in the editor
    private List<GameObject> cardButtons = new List<GameObject>(); // To keep track of instantiated card buttons
    private List<Card> playerHand = new List<Card>();
    private List<bool> heldCards = new List<bool>();

    public Button drawButton; // Reference to the draw button
    public Button dealButton; // Reference to the deal button
    public Button bet1Button, bet5Button; // References to betting buttons
    public TMP_Text resultText; // Text component to display result messages

    void Start()
    {

    }

    public void DealInitialHand()
    {
        ClearHand(); // Clear any existing hand/cards
        playerHand.Clear(); // Clear the current hand

        // Deal 5 new cards from the deck
        for (int i = 0; i < 5; i++)
        {
            Card card = deckManager.DealCard();
            playerHand.Add(card);
            SpawnCardButton(i, card);
        }
        LogCurrentHand();
    }

    private void SpawnCardButton(int index, Card card)
    {
        GameObject cardButtonGO = Instantiate(cardButtonPrefab, cardPositions[index].position, Quaternion.identity, cardPositions[index]);
        cardButtonGO.GetComponent<Image>().sprite = card.cardSprite; // Set the card image
        TMP_Text holdText = cardButtonGO.GetComponentInChildren<TMP_Text>(true); // Assuming there's a TMP_Text for "Held" status
        holdText.gameObject.SetActive(false); // Initially hide the "Held" text

        Button btn = cardButtonGO.GetComponent<Button>();
        btn.onClick.AddListener(() => ToggleHoldCard(index, holdText)); // Add click listener to toggle hold status

        cardButtons.Add(cardButtonGO); // Keep track of the button for later reference
    }

    private void ToggleHoldCard(int cardIndex, TMP_Text holdText)
    {
        // Toggle the hold state visually and logically (if you wish to track this state)
        holdText.gameObject.SetActive(!holdText.gameObject.activeSelf);
    }

    public void DrawNewCards()
    {
        foreach (var cardButtonGO in cardButtons)
        {
            cardButtonGO.GetComponent<Button>().interactable = false; // Disable the button
            cardButtonGO.GetComponent<Image>().color = Color.gray; // Grey out the button
        }

        // Replace non-held cards with new ones from the deck
        for (int i = 0; i < playerHand.Count; i++)
        {
            TMP_Text holdText = cardButtons[i].GetComponentInChildren<TMP_Text>(true);
            if (!holdText.gameObject.activeSelf) // If the card is not held
            {
                Card newCard = deckManager.DealCard(); // Deal a new card
                playerHand[i] = newCard; // Replace the card in the hand
                cardButtons[i].GetComponent<Image>().sprite = newCard.cardSprite; // Update the card's image
            }
        }

        drawButton.interactable = false; // Disable the draw button after use

        LogCurrentHand();
        EvaluateHand();

        // Prepare UI for end game state
    }

    void EvaluateHand()
    {
        var evaluationResult = HandEvaluator.EvaluateHand(playerHand, pokerBettingManager.currentBet);
        string handResult = evaluationResult.handType;
        int winnings = evaluationResult.payout;

        if (winnings > 0)
        {
            // Include the initial bet in the total winnings added to the player's amount
            AmountManager.Instance.AddAmount(winnings + pokerBettingManager.currentBet); // Update total amount with winnings + bet
        }

        LogCurrentHand();
        DisplayResult(handResult, winnings);
    }



    private void DisplayResult(string handResult, int winnings)
    {
        resultText.text = handResult + "! You won " + winnings + " credits.";
        resultText.gameObject.SetActive(true); // Ensure the result text is visible
        //MoveWinningHandIndicator(handResult); // New method to move the indicator
        PrepareEndGameState();
    }

    void HighlightWinningCards()
    {
        // Assuming a simple condition where all cards are highlighted upon any win
        foreach (var cardButtonGO in cardButtons)
        {
            // This finds the highlight Image assumed to be correctly placed in your prefab
            var highlightImage = cardButtonGO.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            highlightImage.gameObject.SetActive(true); // Activate the highlight
        }
    }

    private void PrepareEndGameState()
    {
        // Logic to prepare for the next round
        dealButton.gameObject.SetActive(true); // Re-enable the deal button
        bet1Button.interactable = true; // Re-enable betting buttons
        bet5Button.interactable = true;

        drawButton.gameObject.SetActive(false);
    }

    private void ClearHand()
    {
        // Destroy all card button GameObjects and clear the list
        foreach (GameObject cardButton in cardButtons)
        {
            Destroy(cardButton);
        }
        cardButtons.Clear();
    }

    // Method to reset the game state for a new hand, called by the deal button's OnClick event
    public void ResetForNewHand()
    {
        ClearHand(); // Clear current hand
        dealButton.gameObject.SetActive(false); // Hide deal button for the new hand phase
        drawButton.gameObject.SetActive(true); // Show draw button again for the next hand
        drawButton.interactable = true; // Ensure draw button is interactable
        resultText.gameObject.SetActive(false); // Hide the result text
        // Reset any additional game state as needed
    }

    private void LogCurrentHand()// for debugging purposes. The log will show the pokerValue of the 5 cards currently displayed.
    {
        string handDescription = "Current Hand: ";
        foreach (Card card in playerHand)
        {
            handDescription += $"{card.cardName} of {card.suit} (Value: {card.pokerValue}), ";
        }
        Debug.Log(handDescription.TrimEnd(',', ' ')); // Removes the trailing comma and space
    }
}