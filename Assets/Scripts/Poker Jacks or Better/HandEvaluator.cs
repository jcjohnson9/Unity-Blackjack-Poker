using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HandEvaluator
{
    // Maps hand types to their payout multipliers for each bet amount
    private static readonly Dictionary<string, int[]> Payouts = new Dictionary<string, int[]>
    {
        {"Royal Flush", new[] {250, 500, 750, 1000, 4000}},
        {"Straight Flush", new[] {50, 100, 150, 200, 250}},
        {"Four of a Kind", new[] {25, 50, 75, 100, 125}},
        {"Full House", new[] {9, 18, 27, 36, 45}},
        {"Flush", new[] {6, 12, 18, 24, 30}},
        {"Straight", new[] {4, 8, 12, 16, 20}},
        {"Three of a Kind", new[] {3, 6, 9, 12, 15}},
        {"Two Pair", new[] {2, 4, 6, 8, 10}},
        {"Jacks or Better", new[] {1, 2, 3, 4, 5}}
    };

    public static (string handType, int payout) EvaluateHand(List<Card> hand, int bet)
    {
        // Order hand by pokerValue for accurate straight and high card evaluation
        var orderedHand = hand.OrderBy(card => card.pokerValue).ToList();
        bool isFlush = IsFlush(orderedHand);
        bool isStraight = IsStraight(orderedHand); // Adjusted to consider Aces as high

        if (isFlush && isStraight && orderedHand[0].pokerValue == 10 && orderedHand.Last().pokerValue == 14)
            return ("Royal Flush", Payouts["Royal Flush"][bet - 1]);

        if (isFlush && isStraight)
            return ("Straight Flush", Payouts["Straight Flush"][bet - 1]);

        var groups = orderedHand.GroupBy(card => card.pokerValue).ToList();
        if (groups.Any(g => g.Count() == 4))
            return ("Four of a Kind", Payouts["Four of a Kind"][bet - 1]);

        if (groups.Count == 2 && groups.Any(g => g.Count() == 3))
            return ("Full House", Payouts["Full House"][bet - 1]);

        if (isFlush)
            return ("Flush", Payouts["Flush"][bet - 1]);

        if (isStraight)
            return ("Straight", Payouts["Straight"][bet - 1]);

        if (groups.Any(g => g.Count() == 3))
            return ("Three of a Kind", Payouts["Three of a Kind"][bet - 1]);

        if (groups.Count(g => g.Count() == 2) == 2)
            return ("Two Pair", Payouts["Two Pair"][bet - 1]);

        if (groups.Any(g => g.Count() == 2 && (g.Key >= 11 || g.Key == 14)))
            return ("Jacks or Better", Payouts["Jacks or Better"][bet - 1]);

        return ("No Win", 0);
    }


    private static bool IsFlush(List<Card> hand)
    {
        // Extract the suit from the first card and compare with the rest
        var firstSuit = hand.First().cardName.Substring(0, hand.First().cardName.Length - 2);
        return hand.All(card => card.cardName.StartsWith(firstSuit));
    }

    //5 cards in a sequence
    private static bool IsStraight(List<Card> hand)
    {
        // Ensure the hand is ordered by pokerValue for comparison
        var orderedHand = hand.OrderBy(card => card.pokerValue).ToList();

        // Check for a regular straight first
        for (int i = 0; i < orderedHand.Count - 1; i++)
        {
            if (orderedHand[i].pokerValue + 1 != orderedHand[i + 1].pokerValue)
            {
                // If not a regular straight, check for a wheel straight (A-2-3-4-5)
                return IsWheelStraight(orderedHand);
            }
        }
        return true; // If the loop completes without returning false, it's a straight
    }

    private static bool IsWheelStraight(List<Card> hand)
    {
        // A wheel straight is A-2-3-4-5, which in terms of pokerValue is 14, 2, 3, 4, 5
        // Check specifically for this sequence. Note: The hand must be sorted by pokerValue.
        var values = hand.Select(card => card.pokerValue).OrderBy(v => v).ToList();
        if (values.SequenceEqual(new List<int> { 2, 3, 4, 5, 14 }))
        {
            return true;
        }
        return false;
    }
}