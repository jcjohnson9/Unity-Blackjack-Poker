/*This class handles both the visual aspects of blackjack as well as the logic. 
 * This script should be split in the future to reduce redundancies and confusion*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplayManager : MonoBehaviour
{
    public DeckManager deckManager; // Reference to the DeckManager to get cards
    public GameObject cardPrefab; // Prefab for the card UI
    public Transform playerHand; // Parent transform for player's cards
    public Transform dealerHand; // Parent transform for dealer's cards
    public Transform deckPile; // The position where cards are initially created
    public Sprite faceDownSprite; // Sprite used for face-down cards
    public float cardSpacing = 0.5f; // Adjust as needed for spacing between cards
    public float timeToMove = 0.5f; // Public variable to adjust the slide duration in the inspector

    private GameObject dealerFirstCard; // To keep track of the dealer's first card (face-down)
    private Card dealerFirstCardData; // To store the dealer's first card data for later reveal

    public Button hitButton; // Assign in the inspector
    public TMP_Text messageText;  // Assign in the inspector
    private bool isDealing = false; // To prevent hits while dealing
    public int playerHandValue = 0;

    public Button standButton; // Assign in the inspector
    private int dealerHandValue = 0;

    private int playerAceCount = 0;// keeps track of aces in player's hand
    private int dealerAceCount = 0;// keeps track of aces in dealer's hand
    public bool playerDeal;//The player's turn
    public bool dealerDeal;//The dealer's turn

    //Things that must happen when the scene is opened in
    void Start()
    {
        standButton.interactable = false; // Disable the stand button to prevent multiple clicks
        hitButton.interactable = false; // disable the hit button as well
        hitButton.onClick.AddListener(OnHitButtonClicked);
        StartCoroutine(DealInitialCardsSequentially());
    }

    IEnumerator DealInitialCardsSequentially()
    {
        // Player's first card (face-up)
        playerDeal = true;
        dealerDeal = false;
        Card playerCard1 = deckManager.DealCard();
        playerHandValue += GetCardValue(playerCard1); // Update hand value
        yield return StartCoroutine(DisplayCard(playerCard1, playerHand, false));

        // Dealer's first card (face-down)
        playerDeal = false;
        dealerDeal = true;
        var dealerCard1 = deckManager.DealCard();
        dealerFirstCardData = dealerCard1; // Store card data for revealing later
        yield return StartCoroutine(DisplayCard(dealerCard1, dealerHand, true));

        // Player's second card (face-up)
        playerDeal = true;
        dealerDeal = false;
        Card playerCard2 = deckManager.DealCard();
        playerHandValue += GetCardValue(playerCard2); // Update hand value
        Debug.Log($"Player hand value after initial deal: {playerHandValue}");
        yield return StartCoroutine(DisplayCard(playerCard2, playerHand, false));

        // Dealer's second card (face-up)
        playerDeal = false;
        dealerDeal = true;
        var dealerCard2 = deckManager.DealCard();
        dealerHandValue += GetCardValue(dealerCard2);
        yield return StartCoroutine(DisplayCard(dealerCard2, dealerHand, false));

        // Re-enable buttons for the new round
        hitButton.interactable = true;
        standButton.interactable = true;
        playerDeal = true;
        dealerDeal = false;
        Debug.Log("Payer Card 1 = " + GetCardValue(playerCard1) + " | Player Card 2 = " + GetCardValue(playerCard2));
        Debug.Log("Dealer Card Face Down = "+ GetCardValue(dealerCard1) + " | Dealer Card Face Up = " + GetCardValue(dealerCard2));

        CheckForEndOfRound(); // Check if the initial deal results in win/lose
    }


    IEnumerator DisplayCard(Card card, Transform handTransform, bool isFaceDown)
    {
        // Instantiate the card GameObject at the deckPile position
        GameObject cardGO = Instantiate(cardPrefab, deckPile.position, Quaternion.identity, handTransform);
        SpriteRenderer cardRenderer = cardGO.GetComponent<SpriteRenderer>();
        cardRenderer.sprite = isFaceDown ? faceDownSprite : card.cardSprite;

        // Calculate the target position based on the number of cards already in the hand
        int cardCount = handTransform.childCount - 1; // Subtract one to account for the current card
        Vector3 targetPosition = handTransform.position + new Vector3(cardSpacing * cardCount, 0, 0); // Adjust for orientation

        // Wait for the coroutine to move the card to its target position
        yield return StartCoroutine(MoveCardToPosition(cardGO.transform, targetPosition));

        if (handTransform == dealerHand && isFaceDown)
        {
            dealerFirstCard = cardGO; // Track the dealer's first card if it's face-down
        }
    }

    //method to move cards for both players
    IEnumerator MoveCardToPosition(Transform cardTransform, Vector3 targetPosition)
    {
        float elapsedTime = 0;
        Vector3 startPosition = cardTransform.position;

        while (elapsedTime < timeToMove)
        {
            cardTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardTransform.position = targetPosition; // Ensure the card reaches the target position
    }

    //method for hit button
    public void OnHitButtonClicked()
    {
        if (!isDealing && playerHandValue < 21)
        {
            StartCoroutine(DealCardToPlayer());
        }
    }

    //deals current card to the player
    IEnumerator DealCardToPlayer()
    {
        isDealing = true;
        Card card = deckManager.DealCard();
        // uses cardValue in Card class. This is the value for Blackjack only
        playerHandValue += GetCardValue(card);
        Debug.Log($"Player hand value changed: {playerHandValue}");
        CheckForEndOfRound();
        yield return StartCoroutine(DisplayCard(card, playerHand, false));

        isDealing = false;
    }

    //method to find card value and change ace value, which is 11 by default, to 1.
    int GetCardValue(Card card)
    {
        // Assuming cardValue is already set correctly for faces as 10, and numeric cards as their number
        // For Aces, this returns 1 or 11 based on current hand value;
        if (card.isAce)
        {
            if (playerDeal && playerHandValue + 11 <= 21)//Ace is 11 if player hand is under 21
            {
                playerAceCount++;
                Debug.Log("Player Ace Card Dealt = " + card.cardValue);
                return 11;
            }
            else if (dealerDeal && dealerHandValue + 11 <= 21)//Ace is 11 if dealer hand is under 21
            {
                dealerAceCount++;
                Debug.Log("Dealer Ace Card Dealt = " + card.cardValue);
                return 11;
            }
            return 1; // Default Ace value if adding 11 would bust.
        }
        Debug.Log("Card Dealt = " + card.cardValue);

        return card.cardValue;
    }

    //method for evaluating player hand
    void CheckForEndOfRound()
    {
        Debug.Log("Aces in Player Hand = " + playerAceCount);
        Debug.Log("Aces in Dealer Hand = " + dealerAceCount);
        while (playerHandValue > 21 && playerAceCount > 0)//changes aces already in hand to 1 while hand is over 21.
        {
            playerHandValue -= 10; // Adjust for an Ace counted as 11 previously.
            playerAceCount--; // One less Ace is now counted as 11.
            Debug.Log("Aces in Player Hand Minus 1= " + playerAceCount + "New Hand Value = " + playerHandValue);
        }
        if (playerHandValue == 21)//player has blackjack /////THE DEALER WILL AUTOMATICALLY LOSE in ALL cases. The dealer should check first if they have blackjack. Not the other way around like it is currently.
        {
            playerAceCount = 0;
            dealerAceCount = 0;
            StartCoroutine(ShowMessage("Win!"));
        }
        else if (playerHandValue > 21 && playerAceCount <= 0)//The player bust
        {
            playerAceCount = 0;
            dealerAceCount = 0;
            StartCoroutine(ShowMessage("Lose!"));
        }
    }

    void CollectCardsAndShuffle()
    {
        // Collect cards from player and dealer hands
        foreach (Transform child in playerHand)
        {
            StartCoroutine(MoveCardToDeckPile(child));
        }
        foreach (Transform child in dealerHand)
        {
            StartCoroutine(MoveCardToDeckPile(child));
        }

        // Shuffle the deck after all cards have been moved
        StartCoroutine(ShuffleAndDealNewRound());
    }

    IEnumerator MoveCardToDeckPile(Transform cardTransform)
    {
        yield return StartCoroutine(MoveCardToPosition(cardTransform, deckPile.position));
        Destroy(cardTransform.gameObject); // Optionally destroy the card object or disable it for reuse
    }

    public void OnStandButtonClicked()
    {
        playerDeal = false;
        dealerDeal = true;
        standButton.interactable = false; // Disable the stand button to prevent multiple clicks
        hitButton.interactable = false; //  disable the hit button as well
        StartCoroutine(DealerTurn());
    }

    IEnumerator DealerTurn()
    {
        // Reveal dealer's first card
        if (dealerFirstCard != null && dealerFirstCardData != null)
        {
            dealerFirstCard.GetComponent<SpriteRenderer>().sprite = dealerFirstCardData.cardSprite;
            dealerHandValue += GetCardValue(dealerFirstCardData); // Update dealer hand value
            Debug.Log("Dealer Hand Value = " + dealerHandValue);
            yield return new WaitForSeconds(1f); // Wait for reveal
        }

        // Dealer hits on 16 and below, stands on 17 and above
        while (dealerHandValue <= 16)
        {
            Card card = deckManager.DealCard();
            dealerHandValue += GetCardValue(card);
            // Adjust for the dealer
            while (dealerHandValue > 21 && dealerAceCount > 0)
            {
                dealerHandValue -= 10; // Adjust for an Ace counted as 11 previously.
                dealerAceCount--; // One less Ace is now counted as 11.
            }
            Debug.Log($"Dealer hand value changed: {dealerHandValue}");
            yield return StartCoroutine(DisplayCard(card, dealerHand, false));
            if (dealerHandValue > 21) break; // Check if dealer busts
        }

        // After dealer's turn, check for round outcome
        DetermineOutcome();
    }

    void DetermineOutcome()
    {
        // Implement logic to determine win, lose, or draw
        if (dealerHandValue > 21 || playerHandValue > dealerHandValue)
        {
            playerAceCount = 0;
            dealerAceCount = 0;
            StartCoroutine(ShowMessage("Win!"));
        }
        else if (playerHandValue < dealerHandValue)
        {
            playerAceCount = 0;
            dealerAceCount = 0;
            StartCoroutine(ShowMessage("Lose!"));
        }
        else
        {
            playerAceCount = 0;
            dealerAceCount = 0;
            StartCoroutine(ShowMessage("Draw!"));
        }
    }

    IEnumerator ShuffleDeckAfterDelay()
    {
        // Wait for all cards to finish moving back to the deck pile
        // Adjust the delay based on the time it takes to move cards back
        yield return new WaitForSeconds(1.0f);
        deckManager.ShuffleDeck();
    }

    IEnumerator ShuffleAndDealNewRound()
    {
        yield return new WaitForSeconds(1.0f); // Wait for card collection
        deckManager.ShuffleDeck();
        yield return new WaitForSeconds(1.0f); // Ensure shuffle completes
        StartCoroutine(DealNewRoundAfterDelay()); // Start dealing new round after a brief delay
    }

    IEnumerator ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3); // Show message for 3 seconds
        messageText.gameObject.SetActive(false);

        if (message != "")
        {
            standButton.interactable = false; // Disable the stand button to prevent multiple clicks
            hitButton.interactable = false; // disable the hit button as well
            // Reset for a new round
            StartCoroutine(ResetRoundAfterDelay());
        }
    }

    void ResetRound()
    {

        // Reset hand values and game state
        playerHandValue = 0;
        dealerHandValue = 0;
        dealerFirstCard = null; // Reset reference to dealer's first card
        dealerFirstCardData = null; // Reset dealer's first card data

        // Hide message text
        messageText.gameObject.SetActive(false);

        // Collect cards and prepare for a new round
        CollectCardsAndShuffle();
    }


    IEnumerator ResetRoundAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for message display
        ResetRound(); // Reset round method as previously defined
    }

    IEnumerator DealNewRoundAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); // Adjust based on shuffle and collect delay
        StartCoroutine(DealInitialCardsSequentially());
    }
}