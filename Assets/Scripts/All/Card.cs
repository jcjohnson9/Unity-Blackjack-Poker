using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Sprite cardSprite; //The card sprite
    public int cardValue;// Use for Blackjack value. Aces are Low.(1)
    public string cardName; // For loading sprite from Resources
    public bool isAce = false; //is the card an ace
    public string suit; //Stores the suit of the card
    public int pokerValue; //Stores the poker-specific value or rank. Aces are high(14)
}