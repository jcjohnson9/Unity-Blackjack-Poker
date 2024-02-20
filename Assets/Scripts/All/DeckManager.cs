using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>(); // Used a List for easier manipulation

    void Awake()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    void InitializeDeck()
    {
        deck.Clear(); // Ensure the deck is empty before initializing
        string[] suits = new string[] { "Club", "Diamond", "Heart", "Spade" };
       // int cardIndex = 0;

        foreach (string suit in suits)
        {
            for (int value = 1; value <= 13; value++)
            {
                Card tempCard = ScriptableObject.CreateInstance<Card>();
                string valueString = value.ToString().PadLeft(2, '0'); // Ensure the value is in "01", "02", ... format
                tempCard.cardName = $"{suit}{valueString}";
                tempCard.cardSprite = Resources.Load<Sprite>($"Cards/{tempCard.cardName}");

                // Assign card values and check if it's an Ace
                if (value == 1)
                {
                    tempCard.isAce = true;
                    tempCard.cardValue = value;
                    tempCard.pokerValue = 14; // For Poker, Aces are high
                }
                else if (value > 10) // Jack, Queen, King
                {
                    tempCard.cardValue = 10;
                    tempCard.pokerValue = value; // Retains the same value for Poker (11 for Jack, 12 for Queen, 13 for King)
                }
                else// Numeric cards 2-10
                {
                    tempCard.cardValue = value;
                    tempCard.pokerValue = value; // Numeric cards retain their value in Poker
                    tempCard.isAce = false;
                }

                // Assign the suit for Poker (and potentially other uses)
                tempCard.suit = suit;

                deck.Add(tempCard); // Add the card to the deck
            }
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // Method to deal a single card
    public Card DealCard()
    {
        if (deck.Count > 0)
        {
            Card cardToDeal = deck[0]; // Take the top card
            deck.RemoveAt(0); // Remove it from the deck
            return cardToDeal;
        }
        else
        {
            Debug.LogWarning("Deck is empty!");
            InitializeDeck();
            ShuffleDeck();
            Card cardToDeal = deck[0]; // Take the top card
            deck.RemoveAt(0); // Remove it from the deck
            return cardToDeal; // Return null if the deck is empty
        }
    }
}